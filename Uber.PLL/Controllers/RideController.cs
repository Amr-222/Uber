using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Uber.BLL.Services.Abstraction;
using Uber.BLL.Services.Impelementation;
using Uber.DAL.DataBase;
using Uber.DAL.Entities;

namespace Uber.PLL.Controllers
{
    public class RideController : Controller
    {
        //private readonly IHubContext<RideHub> _hubContext;
        //private readonly IDriverService driverService;
        private readonly IRideService _RideServices;


        public RideController(/*IHubContext<RideHub> hubContext, IDriverService driverService*/ IRideService rideService)
        {
            //_hubContext = hubContext;
            //this.driverService = driverService;
            _RideServices = rideService;
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
                _RideServices.Create(ride);
                return RedirectToAction(nameof(Index));
            }
            return View(ride);
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
}
