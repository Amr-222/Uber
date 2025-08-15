using Microsoft.AspNetCore.Mvc;
using Uber.BLL.Services.Abstraction;
using Uber.BLL.ModelVM.Driver;
using Uber.DAL.Entities;
using Uber.BLL.ModelVM.User;

namespace Uber.PLL.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDriverService _driverService;
        private readonly IRideService _rideService;
        private readonly IUserService _userService;

        public AdminController(IDriverService driverService, IRideService rideService, IUserService userService)
        {
            _driverService = driverService;
            _rideService = rideService;
            _userService = userService;
        }
        public IActionResult AdminDashBoard()
        {
            return View();
        }
        public IActionResult Admin_Drivers()
        {
            var drivers = _driverService.GetAll();
            return View(drivers);
        }

        [HttpGet]
        public IActionResult EditDriver(string id)
        {
            var driver = _driverService.GetById(id);
            if (driver == null)
                return NotFound();

            return View(driver);
        }

        [HttpPost]
        public IActionResult EditDriver(EditDriver driver)
        {
            if (ModelState.IsValid)
            {
                _driverService.EditDriver(driver);
                return RedirectToAction("Admin_Drivers");
            }
            return View(driver);
        }

        [HttpPost]
        public IActionResult DeleteDriver(string id)
        {
            _driverService.Delete(id);
            return RedirectToAction("Admin_Drivers");
        }
        public IActionResult Admin_Riders()
        {
            var users = _userService.GetAll();
            return View(users);
        }

        [HttpGet]
        public IActionResult EditRider(string id)
        {
            var user = _userService.GetByIDToEdit(id);
            if (user.Item2 == null)
                return NotFound();

            return View("EditUser", user.Item2);
        }

        [HttpPost]
        public IActionResult EditRider(EditUser user)
        {
            if (ModelState.IsValid)
            {
                _userService.Edit(user);
                return RedirectToAction("Admin_Riders");
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteRider(string id)
        {
            _userService.Delete(id);
            return RedirectToAction("Admin_Riders");
        }
    }
}
