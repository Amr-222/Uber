using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
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
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = service.MakeUserActive(Id);
            return View();
        }

        [HttpGet]
        public IActionResult GetCurrentDriverId()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }
                
                return Json(new { driverId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult MakeUserInactive()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var result = service.MakeUserInactive(userId);
                if (result.Item1)
                {
                    return Json(new { success = true, message = "Driver status set to inactive" });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Item2 });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult MakeUserActive()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var result = service.MakeUserActive(userId);
                if (result.Item1)
                {
                    return Json(new { success = true, message = "Driver status set to active" });
                }
                else
                {
                    return BadRequest(new { success = false, message = result.Item2 });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Error: {ex.Message}" });
            }
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
