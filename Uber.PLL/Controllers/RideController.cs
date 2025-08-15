using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Uber.BLL.ModelVM.Ride;
using Uber.BLL.Services.Abstraction;
using Uber.DAL.Entities;
using Uber.DAL.Enums;

namespace Uber.PLL.Controllers
{
    // Model for ride request from Home page
    public class RideRequestModel
    {
        public double StartLat { get; set; }
        public double StartLng { get; set; }
        public double EndLat { get; set; }
        public double EndLng { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
        public double Price { get; set; }
    }

    // Model for rating requests
    public class RatingRequest
    {
        public int RideId { get; set; }
        public int Rating { get; set; }
    }

    public class RideController : Controller
    {
        private readonly IHubContext<RideHub> _hub;
        private readonly IDriverService _driverService;
        private readonly IRideService _rideService;

        public RideController(IHubContext<RideHub> hub, IDriverService driverService, IRideService rideService)
        {
            _hub = hub;
            _driverService = driverService;
            _rideService = rideService;
        }

        [Authorize] // user must be logged in
        [HttpGet]
        public async Task<IActionResult> RequestRide(double StartLat, double StartLng, double EndLat, double EndLng,
            double Distance, double Duration, double Price)
        {
            try
            {
                // 1) find nearest driver (you already have GetNearestDriver)
                var nearest = _driverService.GetNearestDriver(StartLat, StartLng);
                if (!nearest.Item1 || nearest.Item3 == null || !nearest.Item3.Any())
                {
                    return View("NoDrivers");
                }

                var chosenDriverId = nearest.Item3.FirstOrDefault(); // IdentityUser.Id (string)
                if (string.IsNullOrEmpty(chosenDriverId))
                {
                    return View("NoDrivers");
                }

                // 2) create pending ride in DB
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var (ok, err, ride) = _rideService.CreatePendingRide(userId, chosenDriverId, StartLat, StartLng, EndLat, EndLng, Distance, Duration, Price);
                if (!ok || ride == null)
                {
                    return BadRequest(err ?? "Failed to create ride");
                }

                var rideGroup = $"ride-{ride.Id}";

                // 3) Notify the target driver - use a driver-specific group instead of user ID
                var driverGroup = $"driver-{chosenDriverId}";
                Console.WriteLine($"Sending ride request to driver group: {driverGroup}");
                Console.WriteLine($"Ride data: {System.Text.Json.JsonSerializer.Serialize(new { rideId = ride.Id, rideGroup, startLat = StartLat, startLng = StartLng, endLat = EndLat, endLng = EndLng, userId })}");

                await _hub.Clients.Group(driverGroup).SendAsync("ReceiveRideRequest", new
                {
                    rideId = ride.Id,
                    rideGroup,
                    startLat = StartLat,
                    startLng = StartLng,
                    endLat = EndLat,
                    endLng = EndLng,
                    userId
                });

                Console.WriteLine($"Ride request sent successfully to driver {chosenDriverId}");

                // 4) Show waiting view (client will connect to hub & join group)
                return View("WaitingForDriver", ride.Id.ToString());
            }
            catch (Exception ex)
            {
                // Log the exception here
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize] // user must be logged in
        [HttpPost]
        public async Task<IActionResult> RequestRide([FromBody] RideRequestModel request)
        {
            try
            {
                // 1) find nearest driver
                var nearest = _driverService.GetNearestDriver(request.StartLat, request.StartLng);
                if (!nearest.Item1 || nearest.Item3 == null || !nearest.Item3.Any())
                {
                    return BadRequest(new { message = "No drivers available in your area" });
                }

                var chosenDriverId = nearest.Item3.FirstOrDefault();
                if (string.IsNullOrEmpty(chosenDriverId))
                {
                    return BadRequest(new { message = "No drivers available in your area" });
                }

                // 2) create pending ride in DB
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var (ok, err, ride) = _rideService.CreatePendingRide(userId, chosenDriverId, request.StartLat, request.StartLng, request.EndLat, request.EndLng, request.Distance, request.Duration, request.Price);
                if (!ok || ride == null)
                {
                    return BadRequest(new { message = err ?? "Failed to create ride" });
                }

                var rideGroup = $"ride-{ride.Id}";

                // 3) Notify the target driver
                var driverGroup = $"driver-{chosenDriverId}";
                Console.WriteLine($"Sending ride request to driver group: {driverGroup}");
                Console.WriteLine($"Ride data: {System.Text.Json.JsonSerializer.Serialize(new { rideId = ride.Id, rideGroup, startLat = request.StartLat, startLng = request.StartLng, endLat = request.EndLat, endLng = request.EndLng, userId })}");

                await _hub.Clients.Group(driverGroup).SendAsync("ReceiveRideRequest", new
                {
                    rideId = ride.Id,
                    rideGroup,
                    startLat = request.StartLat,
                    startLng = request.StartLng,
                    endLat = request.EndLat,
                    endLng = request.EndLng,
                    userId
                });

                Console.WriteLine($"Ride request sent successfully to driver {chosenDriverId}");

                // 4) Return JSON response with ride ID for the Home page
                return Ok(new
                {
                    success = true,
                    message = "Ride request created successfully",
                    rideId = ride.Id,
                    rideGroup = rideGroup
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RequestRide POST: {ex.Message}");
                return BadRequest(new { message = $"An error occurred: {ex.Message}" });
            }
        }

        // These can be called by Hub as well, but keeping REST fallbacks:
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DriverAccept([FromBody] AcceptRejectRequest request)
        {
            try
            {
                Console.WriteLine($"DriverAccept called with: id={request.id}, rideGroup={request.rideGroup}");

                var (ok, err) = _rideService.MarkAccepted(request.id);
                if (!ok)
                {
                    Console.WriteLine($"Failed to mark ride as accepted: {err}");
                    return BadRequest(err ?? "Failed to accept ride");
                }

                Console.WriteLine($"Ride marked as accepted successfully. Sending notification to group: {request.rideGroup}");

                // Send notification to the ride group
                await _hub.Clients.Group(request.rideGroup).SendAsync("RideAccepted", request.id);

                Console.WriteLine($"Notification sent successfully to group: {request.rideGroup}");

                return Ok(new { success = true, message = "Ride accepted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DriverAccept: {ex.Message}");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DriverReject([FromBody] AcceptRejectRequest request)
        {
            try
            {
                var (ok, err) = _rideService.MarkRejected(request.id);
                if (!ok)
                {
                    return BadRequest(err ?? "Failed to reject ride");
                }

                // Get the rejected ride to find next nearest driver
                var (rideErr, ride) = _rideService.GetByID(request.id);
                if (rideErr != null || ride == null)
                {
                    return BadRequest("Ride not found");
                }

                // Find next nearest driver
                var nextNearest = _driverService.GetNearestDriver(ride.StartLat, ride.StartLng);
                if (nextNearest.Item1 && nextNearest.Item3 != null && nextNearest.Item3.Any())
                {
                    // Skip the driver who just rejected
                    var nextDriverId = nextNearest.Item3.FirstOrDefault(d => d != ride.DriverId);

                    if (!string.IsNullOrEmpty(nextDriverId))
                    {
                        // Update ride with new driver
                        var (updateOk, updateErr) = _rideService.AssignNewDriver(request.id, nextDriverId);
                        if (updateOk)
                        {
                            // Notify the new driver
                            await _hub.Clients.Group($"driver-{nextDriverId}").SendAsync("ReceiveRideRequest", new
                            {
                                rideId = ride.Id,
                                rideGroup = request.rideGroup,
                                startLat = ride.StartLat,
                                startLng = ride.StartLng,
                                endLat = ride.EndLat,
                                endLng = ride.EndLng,
                                userId = ride.UserId
                            });

                            // Notify user that request was sent to another driver
                            await _hub.Clients.Group(request.rideGroup).SendAsync("RideRejected", request.id);
                            return Ok(new { success = true, message = "Ride rejected, sent to next driver" });
                        }
                    }
                }

                // If no more drivers available, notify user
                await _hub.Clients.Group(request.rideGroup).SendAsync("RideRejected", request.id);
                return Ok(new { success = true, message = "Ride rejected, no more drivers available" });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ride ride)
        {
            if (ModelState.IsValid)
            {
                _rideService.Create(ride);
                return RedirectToAction(nameof(Index));
            }
            return View(ride);
        }

        public IActionResult Request()
        {
            return View();
        }

        public IActionResult NoDrivers()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        [Authorize]
        public IActionResult RideDetails(int id)
        {
            try
            {
                var (err, ride) = _rideService.GetByID(id);
                if (err != null || ride == null)
                {
                    return NotFound("Ride not found");
                }

                // Check if the current user is authorized to view this ride
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized("User not authenticated");
                }

                // User can view if they are the ride requester or the assigned driver
                if (ride.UserId != currentUserId && ride.DriverId != currentUserId)
                {
                    return Forbid("You are not authorized to view this ride");
                }

                return View(ride);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPost]
        public async Task<IActionResult> StartRide([FromBody] AcceptRejectRequest request)
        {
            try
            {
                var (ok, err) = _rideService.MarkInProgress(request.id);
                if (!ok)
                {
                    return BadRequest(err ?? "Failed to start ride");
                }

                // Notify both user and driver that ride has started
                await _hub.Clients.Group($"ride-{request.id}").SendAsync("RideStarted", request.id);
                return Ok(new { success = true, message = "Ride started successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPost]
        public async Task<IActionResult> CompleteRide([FromBody] AcceptRejectRequest request)
        {
            try
            {
                var (ok, err) = _rideService.MarkCompleted(request.id);
                if (!ok)
                {
                    return BadRequest(err ?? "Failed to complete ride");
                }

                // Notify both user and driver that ride has completed
                await _hub.Clients.Group($"ride-{request.id}").SendAsync("RideCompleted", request.id);
                return Ok(new { success = true, message = "Ride completed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPost]
        public async Task<IActionResult> DriverArrived([FromBody] AcceptRejectRequest request)
        {
            try
            {
                Console.WriteLine($"DriverArrived called with ride ID: {request.id}");
                
                var (ok, err) = _rideService.MarkDriverWaiting(request.id);
                if (!ok)
                {
                    Console.WriteLine($"Failed to mark driver as arrived: {err}");
                    return BadRequest(err ?? "Failed to mark driver as arrived");
                }

                Console.WriteLine($"Driver marked as arrived successfully for ride {request.id}");

                // Notify both user and driver that driver has arrived
                await _hub.Clients.Group($"ride-{request.id}").SendAsync("DriverArrived", request.id);
                return Ok(new { success = true, message = "Driver arrived at pickup location" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DriverArrived: {ex.Message}");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddUserRating([FromBody] RatingRequest request)
        {
            try
            {
                var (ok, err) = _rideService.AddUserRating(request.RideId, request.Rating);
                if (!ok)
                {
                    return BadRequest(err ?? "Failed to add user rating");
                }

                // Notify both user and driver that user rating was added
                await _hub.Clients.Group($"ride-{request.RideId}").SendAsync("UserRated", request.RideId, request.Rating);
                return Ok(new { success = true, message = "User rating added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddDriverRating([FromBody] RatingRequest request)
        {
            try
            {
                var (ok, err) = _rideService.AddDriverRating(request.RideId, request.Rating);
                if (!ok)
                {
                    return BadRequest(err ?? "Failed to add driver rating");
                }

                // Notify both user and driver that driver rating was added
                await _hub.Clients.Group($"ride-{request.RideId}").SendAsync("DriverRated", request.RideId, request.Rating);
                return Ok(new { success = true, message = "Driver rating added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CancelRide([FromBody] AcceptRejectRequest request)
        {
            try
            {
                var (ok, err) = _rideService.Cancel(request.id);
                if (!ok)
                {
                    return BadRequest(err ?? "Failed to cancel ride");
                }

                // Notify both user and driver that ride has been cancelled
                await _hub.Clients.Group($"ride-{request.id}").SendAsync("RideCancelled", request.id);
                
                // Also notify the driver specifically if they exist
                var (rideErr, ride) = _rideService.GetByID(request.id);
                if (rideErr == null && ride != null && !string.IsNullOrEmpty(ride.DriverId))
                {
                    await _hub.Clients.Group($"driver-{ride.DriverId}").SendAsync("RideCancelled", request.id);
                }
                
                return Ok(new { success = true, message = "Ride cancelled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        // Test method to verify SignalR is working
        [HttpPost]
        public async Task<IActionResult> TestSignalR([FromBody] AcceptRejectRequest request)
        {
            try
            {
                Console.WriteLine($"Testing SignalR for ride {request.id}");

                // Send a test message to the ride group
                await _hub.Clients.Group($"ride-{request.id}").SendAsync("TestMessage", $"Test message for ride {request.id}");

                Console.WriteLine($"Test message sent successfully");
                return Ok(new { success = true, message = "Test message sent successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TestSignalR: {ex.Message}");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        public IActionResult WaitingForDriver(int? id = null)
        {
            try
            {
                // If an ID is provided, pass it to the view
                if (id.HasValue)
                {
                    return View( model: id.ToString());
                }

                // Otherwise, just show the waiting page without specific ride ID
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }

}
