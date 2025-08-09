using Microsoft.AspNetCore.Mvc;
using Uber.DAL.Entities;
using Uber.DAL.DataBase;
using Uber.BLL.Services.Impelementation;

namespace Uber.PLL.Controllers
{
    public class RideController : Controller
    {
        private readonly RideService _RideServices;

        public RideController(RideService _RideServices)
        {
            this._RideServices = _RideServices;
        }

        public IActionResult Index()
        {
            var rides = _RideServices.GetAll();
            return View(rides);
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
