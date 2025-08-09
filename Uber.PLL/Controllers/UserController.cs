using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Uber.BLL.ModelVM.User;
using Uber.BLL.Services.Abstraction;
using Uber.BLL.Services.Impelementation;
using Uber.DAL.DataBase;
using Uber.DAL.Entities;

namespace Uber.PLL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
             _userService = userService;
        }


        [HttpGet]
        public Microsoft.AspNetCore.Mvc.IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public Microsoft.AspNetCore.Mvc.IActionResult Register(CreateUser model)
        {
            if (ModelState.IsValid)
            {
               _userService.Create(model);
               return RedirectToAction("Login", "User");
            }
            return View(model);

        }

        [HttpGet]
        public Microsoft.AspNetCore.Mvc.IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public Microsoft.AspNetCore.Mvc.IActionResult Login(string email, string password, User user)
        //{
        //    User? User = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

        //    if (User != null)
        //    {
        //        HttpContext.Session.SetInt32("UserId", User.Id);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ViewBag.Error = "Invalid email or password";
        //    return View();
        //}
        [HttpGet]
        public Microsoft.AspNetCore.Mvc.IActionResult Edit(string id)
        {
            var result = _userService.GetByID(id);
            var user = result.Item2;
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public Microsoft.AspNetCore.Mvc.IActionResult Edit(EditUser model)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Edit(model);
                if (result.Item1)
                    return RedirectToAction("Index", "Home");

                ModelState.AddModelError("", result.Item2);
            }
            return View(model);
        }

    }
}
