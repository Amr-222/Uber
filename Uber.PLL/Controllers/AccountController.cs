using Microsoft.AspNetCore.Mvc;

namespace Uber.PLL.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChooseLoginType()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string role)
        {
            if (role == "Driver")
                return View("~/Views/Driver/Login.cshtml");

            if (role == "User")
                return View("~/Views/User/Login.cshtml");

            return NotFound();
        }

        public IActionResult ChooseRegisterType()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register(string role)
        {
            if (role == "Driver")
                return View("~/Views/Driver/Register.cshtml");

            if (role == "User")
                return View("~/Views/User/Register.cshtml");

            return NotFound();
        }

    }
}