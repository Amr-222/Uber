using Microsoft.AspNetCore.Mvc;
using Uber.DAL.DataBase; 
using Uber.DAL.Entities;
using Uber.BLL.Services.Abstraction;
using Uber.BLL.Services.Impelementation;
using Uber.BLL.ModelVM.Driver;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Uber.PLL.Controllers
{
    public class DriverController : Controller
    {
        private readonly IDriverService service;

        public DriverController(DriverService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult RegisterDriver()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterDriver(CreateDriver model, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Photo.CopyTo(fileStream);
                    }

                    model.ImagePath = "/uploads/" + uniqueFileName;
                }

                service.Create(model);
                

                return RedirectToAction("Login", "Driver");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(string email, string password)
        //{
        //    var driver = service.Drivers.FirstOrDefault(d => d.Email == email && d.Password == password);
        //    if (driver != null)
        //    {
        //        HttpContext.Session.SetInt32("DriverId", driver.Id);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ViewBag.Error = "Invalid email or password";
        //    return View();
        //}

    }
}
