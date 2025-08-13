using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Uber.BLL.Services.Abstraction;
using Uber.BLL.Services.Impelementation;
using Uber.DAL.Entities;

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
    public async Task<IActionResult> RequestRide(
    double StartLat, double StartLng, double EndLat, double EndLng)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var nearestDriversResult = _driverService.GetNearestDriver(StartLat, StartLng);
        if (!nearestDriversResult.Item1 || nearestDriversResult.Item3 == null || !nearestDriversResult.Item3.Any())
            return View("NoDrivers");

        var nearestDriver = nearestDriversResult.Item3[0];

        // Create pending ride in DB
        var (ok, err, ride) = _rideService.CreatePendingRide(
            userId,
            nearestDriver.Id, // This is Driver.Id
            StartLat,
            StartLng,
            EndLat,
            EndLng
        );

        if (!ok || ride == null)
            return BadRequest(err);

        // Send ride request to the driver via SignalR
        await _hub.Clients.User(nearestDriver.Id)
            .SendAsync("ReceiveRideRequest", new
            {
                RideId = ride.Id,
                StartLat,
                StartLng,
                EndLat,
                EndLng
            });

        return View("WaitingForDriver", ride.Id.ToString()); // Pass RideId to the view
    }



// These can be called by Hub as well, but keeping REST fallbacks:
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DriverAccept(int id, string rideGroup)
    {
        _rideService.MarkAccepted(id);
        await _hub.Clients.Group(rideGroup).SendAsync("RideAccepted", id);
        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DriverReject(int id, string rideGroup)
    {
        _rideService.MarkRejected(id);
        await _hub.Clients.Group(rideGroup).SendAsync("RideRejected", id);
        return Ok();
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

        //public IActionResult Delete(int id)
        //{
        //    var ride = _context.Rides.Find(id);
        //    if (ride == null) return NotFound();

        //    _context.Rides.Remove(ride);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}


}

