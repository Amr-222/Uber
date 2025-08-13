using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Uber.BLL.ModelVM.Account;
using Uber.BLL.ModelVM.Driver;
using Uber.BLL.Services.Abstraction;
using Uber.BLL.Services.Impelementation;
using Uber.DAL.DataBase; 
using Uber.DAL.Entities;

namespace Uber.PLL.Controllers
{
    public class DriverController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDriverService service;

        public DriverController(IDriverService service, SignInManager<ApplicationUser> signInManager)
        {
            this.service = service;
            this.signInManager = signInManager;
        }


        [HttpPost]
        public async Task<IActionResult> RegisterDriver(CreateDriver driver)
        {
            if (ModelState.IsValid)
            {
                var (success, error) = await service.CreateAsync(driver);

                if (!success)
                {
                    ModelState.AddModelError("", error ?? "An error occurred");
                    return View("~/Views/Driver/Register.cshtml", driver);
                }

                return RedirectToAction("Login", "Driver");
            }

            return View("~/Views/Driver/Register.cshtml", driver);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Login(LoginVM model)
        {



            var result = await signInManager.PasswordSignInAsync(
     model.Email,      
     model.Password,    
     model.RememberMe,
     lockoutOnFailure: false
 );


            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                ViewBag.LoginError = "*Invalid username or password";
                return View();
            }
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AcceptRide(string rideId)
        {
            // Logic to mark ride as accepted
            // Notify user through SignalR if needed

            return Json(new { success = true });
        }
    }
}
