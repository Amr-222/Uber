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
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IDriverService service;

        public DriverController(IDriverService service, SignInManager<IdentityUser> signInManager)
        {
            this.service = service;
            this.signInManager = signInManager;
        }

       
        public IActionResult RegisterDriver(CreateDriver driver)
        {
            if (ModelState.IsValid)
            {
                //if (Photo != null && Photo.Length > 0)
                //{
                //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                //    if (!Directory.Exists(uploadsFolder))
                //    {
                //        Directory.CreateDirectory(uploadsFolder);
                //    }

                //    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //    using (var fileStream = new FileStream(filePath, FileMode.Create))
                //    {
                //        Photo.CopyTo(fileStream);
                //    }

                //    model.ImagePath = "/uploads/" + uniqueFileName;
                //}





                service.CreateAsync(driver);
                

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


            return View("/Views/Home/Index.cshtml");
        }

    }
}
