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

        public UserController(IUserService service, SignInManager<ApplicationUser> signInManager)
        {
            this.service = service;
            this.signInManager = signInManager;
        }


        [HttpPost]
        public async Task<IActionResult> RegisterDriver(CreateUser driver)
        {
            if (ModelState.IsValid)
            {
                var (success, error) = await service.CreateAsync(driver);

                if (!success)
                {
                    ModelState.AddModelError("", error ?? "An error occurred");
                    return View("~/Views/User/Register.cshtml", driver);
                }

                return RedirectToAction("Login", "User");
            }

            return View("~/Views/User/Register.cshtml", driver);
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


            return View("/Views/Home/Index.cshtml");
        }

    }
}
