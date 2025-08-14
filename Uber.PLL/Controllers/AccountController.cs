using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Uber.BLL.ModelVM.Account;
using Uber.DAL.Entities;
namespace Uber.PLL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult ChooseLoginType() => View();
        public IActionResult ChooseRegisterType() => View();

        //[HttpGet]
        //public IActionResult Login(string role)
        //{
        //    if (role == "Driver")
        //        return View("~/Views/Driver/Login.cshtml");

        //    if (role == "User")
        //        return View("~/Views/User/Login.cshtml");

        //    return NotFound();
        //}
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false
            );


            if (result.Succeeded)
            {
                if (User.IsInRole("User"))
                    return RedirectToAction("Index", "Home");
                else if (User.IsInRole("Driver"))
                    return RedirectToAction("Dashboard", "Driver");
                else if (User.IsInRole("Admin"))
                    return RedirectToAction("AdminDashBoard", "Admin");
                else
                    return View();
            }
            else
            {

                ViewBag.LoginError = "*Invalid username or password";
                return View();
            }
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

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {                    
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("Password", item.Description);
                }
            }
            return View(model);
        }
    }
}
