using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Uber.BLL.Services.Abstraction;
using Uber.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
    public async Task<IActionResult> RequestRide(double StartLat, double StartLng, double EndLat, double EndLng)
    {
        // 1) find nearest driver (you already have GetNearestDriver)
        var nearest = _driverService.GetNearestDriver(StartLat, StartLng);
        if (!nearest.Item1) return View("NoDrivers");

        var chosenDriverId = nearest.Item3.FirstOrDefault(); // IdentityUser.Id (string)

        // 2) create pending ride in DB
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var (ok, err, ride) = _rideService.CreatePendingRide(userId!, chosenDriverId, StartLat, StartLng, EndLat, EndLng);
        if (!ok || ride == null) return BadRequest(err);

        var rideGroup = $"ride-{ride.Id}";

        // 3) Notify the target driver
        await _hub.Clients.User(chosenDriverId).SendAsync("ReceiveRideRequest", new
        {
            rideId = ride.Id,
            rideGroup,
            startLat = StartLat,
            startLng = StartLng,
            endLat = EndLat,
            endLng = EndLng,
            userId
        });

        // 4) Show waiting view (client will connect to hub & join group)
        return View("WaitingForDriver", ride.Id.ToString());
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

