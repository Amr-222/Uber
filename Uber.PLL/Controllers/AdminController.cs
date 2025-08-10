using Microsoft.AspNetCore.Mvc;

namespace Uber.PLL.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminDashBoard()
        {
            return View();
        }

        public IActionResult Admin_Drivers()
        {
            return View();
        }

        public IActionResult Admin_Riders()
        {
            return View();
        }

        public IActionResult Admin_Rides()
        {
            return View();
        }

        public IActionResult Admin_Admines()
        {
            return View();
        }
    }
}
