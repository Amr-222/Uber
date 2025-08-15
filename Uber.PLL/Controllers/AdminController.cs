using Microsoft.AspNetCore.Mvc;
using Uber.BLL.Services.Abstraction;
using Uber.BLL.ModelVM.Driver;
using Uber.DAL.Entities;
using Uber.BLL.ModelVM.User;
using Uber.BLL.ModelVM.Admin;

namespace Uber.PLL.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDriverService _driverService;
        private readonly IRideService _rideService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
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
            var driver = _driverService.GetByIDToEdit(id);
            if (driver.Item2 == null)
                return NotFound();

            return View(driver.Item2);
        }

        [HttpPost]
        public IActionResult EditDriver(EditDriver driver)
        {
            if (ModelState.IsValid)
            {
               var res=  _driverService.Edit(driver);
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






        public IActionResult Admin_Admins()
        {
            var admins = _adminService.GetAll();
            return View(admins);
        }

        [HttpGet]
        public IActionResult EditAdmin(string id)
        {
            var admin = _adminService.GetByIDToEdit(id);
            if (admin.Item2 == null)
                return NotFound();

            return View("EditAdmin", admin.Item2); // Make The EditAdmin VIEW
        }

        [HttpPost]
        public IActionResult EditAdmin(EditAdmin admin)
        {
            if (ModelState.IsValid)
            {
                _adminService.Edit(admin);
                return RedirectToAction("Admin_Admins");
            }
            return View(admin);
        }

        [HttpPost]
        public IActionResult DeleteAdmin(string id)
        {
            _adminService.Delete(id);
            return RedirectToAction("Admin_Admins");
        }





    }
}
