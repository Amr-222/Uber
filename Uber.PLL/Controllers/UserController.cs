using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Uber.BLL.ModelVM.Account;
using Uber.BLL.ModelVM.Driver;
using Uber.BLL.ModelVM.User;
using Uber.BLL.Services.Abstraction;
using Uber.BLL.Services.Impelementation;
using Uber.DAL.DataBase;
using Uber.DAL.Entities;

namespace Uber.PLL.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService service;
        private readonly IDriverService driverService;

        public UserController(IUserService service, SignInManager<ApplicationUser> signInManager, IDriverService driverService)
        {
            this.service = service;
            this.signInManager = signInManager;
            this.driverService = driverService;
        }



        [HttpPost]
        public async Task<IActionResult> RegisterUser(CreateUser user)
        {
            if (ModelState.IsValid)
            {
                var (success, error) = await service.CreateAsync(user);

                if (!success)
                {
                    ModelState.AddModelError("", error ?? "An error occurred");
                    return View("~/Views/User/Register.cshtml", user);
                }

                return RedirectToAction("Login", "User");
            }

            return View("~/Views/User/Register.cshtml", user);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
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

        [HttpGet]
        public IActionResult RequestRide()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var result = await service.GetProfileInfo();
            return View(result.Item3);
        }



        //[HttpPost]
        /*public IActionResult RequestRide(double StartLat, double StartLng, double EndLat, double EndLng, string Id)
        {
            var result = driverService.GetNearstDriver(StartLat, StartLng);
            if (!result.Item1) return View(result.Item2);
            bool foundDriver = false;
            int index = 0;
            while (!foundDriver)
            {
                var res = driverService.SendRequest(result.Item3[index++], Id);
            }
        }*/
    }
}
