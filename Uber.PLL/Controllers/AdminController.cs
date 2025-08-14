using Microsoft.AspNetCore.Mvc;
using Uber.BLL.Services.Abstraction;
using Uber.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Uber.PLL.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDriverService _driverService;
        private readonly IRideService _rideService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IDriverService driverService, IRideService rideService, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _driverService = driverService;
            _rideService = rideService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AdminDashBoard()
        {
            return View();
        }

        public IActionResult Admin_Drivers()
        {
            var drivers = _driverService.GetAllDrivers();
            return View(drivers);
        }

        public IActionResult Admin_Riders()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        public IActionResult Admin_Rides()
        {
            var rides = _rideService.GetAll();
            return View(rides);
        }

        public IActionResult RideDetails(int id)
        {
            var result = _rideService.GetByID(id);
            if (result.Item1 != null)
            {
                TempData["ErrorMessage"] = $"Ride not found: {result.Item1}";
                return RedirectToAction("Admin_Rides");
            }

            var ride = result.Item2;
            if (ride == null)
            {
                TempData["ErrorMessage"] = "Ride not found.";
                return RedirectToAction("Admin_Rides");
            }

            return View(ride);
        }

        public IActionResult Admin_Admines()
        {
            return View();
        }

        public IActionResult EditDriver(string id)
        {
            var result = _driverService.GetByID(id);
            if (result.Item1 != null)
            {
                TempData["ErrorMessage"] = $"Driver not found: {result.Item1}";
                return RedirectToAction("Admin_Drivers");
            }

            var driver = result.Item2;
            if (driver == null)
            {
                TempData["ErrorMessage"] = "Driver not found.";
                return RedirectToAction("Admin_Drivers");
            }

            return View(driver);
        }

        [HttpPost]
        public IActionResult EditDriver(string id, string Name, DateTime DateOfBirth, IFormFile ProfilePhoto, 
            string VehicleBrand, string VehicleModel, int VehicleYear, string VehiclePlate, 
            string VehicleColor, int VehicleSeatingCapacity, bool IsActive)
        {
            try
            {
                var result = _driverService.GetByID(id);
                if (result.Item1 != null)
                {
                    TempData["ErrorMessage"] = $"Driver not found: {result.Item1}";
                    return RedirectToAction("Admin_Drivers");
                }

                var driver = result.Item2;
                if (driver == null)
                {
                    TempData["ErrorMessage"] = "Driver not found.";
                    return RedirectToAction("Admin_Drivers");
                }

                // Handle profile photo upload
                string imagePath = driver.ImagePath;
                if (ProfilePhoto != null && ProfilePhoto.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Files");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePhoto.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ProfilePhoto.CopyTo(fileStream);
                    }

                    imagePath = uniqueFileName;
                }

                // Update driver information
                var editResult = driver.Edit(Name, DateOfBirth, imagePath);
                if (!editResult.Item1)
                {
                    TempData["ErrorMessage"] = $"Failed to update driver: {editResult.Item2}";
                    return RedirectToAction("EditDriver", new { id });
                }

                // Update vehicle information if available
                if (driver.Vehicle != null)
                {
                    driver.Vehicle.Brand = VehicleBrand ?? driver.Vehicle.Brand;
                    driver.Vehicle.Model = VehicleModel ?? driver.Vehicle.Model;
                    driver.Vehicle.YearMade = VehicleYear > 0 ? VehicleYear : driver.Vehicle.YearMade;
                    driver.Vehicle.Plate = VehiclePlate ?? driver.Vehicle.Plate;
                    driver.Vehicle.Color = VehicleColor ?? driver.Vehicle.Color;
                    driver.Vehicle.SeatingCapacity = VehicleSeatingCapacity > 0 ? VehicleSeatingCapacity : driver.Vehicle.SeatingCapacity;
                }

                // Update driver status
                if (IsActive != driver.IsActive)
                {
                    if (IsActive)
                    {
                        driver.MakeActive();
                    }
                    else
                    {
                        driver.MakeInactive();
                    }
                }

                // Save changes through repository
                var saveResult = _driverService.UpdateDriver(driver);
                if (!saveResult.Item1)
                {
                    TempData["ErrorMessage"] = $"Failed to save changes: {saveResult.Item2}";
                    return RedirectToAction("EditDriver", new { id });
                }

                TempData["SuccessMessage"] = "Driver information updated successfully!";
                return RedirectToAction("Admin_Drivers");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("EditDriver", new { id });
            }
        }

        [HttpPost]
        public IActionResult DeleteDriver(string id)
        {
            var result = _driverService.Delete(id);
            if (result.Item1)
            {
                TempData["SuccessMessage"] = "Driver deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to delete driver: {result.Item2}";
            }
            return RedirectToAction("Admin_Drivers");
        }

        [HttpPost]
        public IActionResult ToggleDriverStatus(string id, bool makeActive)
        {
            var result = makeActive ? _driverService.MakeUserActive(id) : _driverService.MakeUserInactive(id);
            if (result.Item1)
            {
                TempData["SuccessMessage"] = $"Driver {(makeActive ? "activated" : "deactivated")} successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to {(makeActive ? "activate" : "deactivate")} driver: {result.Item2}";
            }
            return RedirectToAction("Admin_Drivers");
        }

        [HttpPost]
        public IActionResult CancelRide(int id)
        {
            var result = _rideService.Cancel(id);
            if (result.Item1)
            {
                TempData["SuccessMessage"] = "Ride cancelled successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to cancel ride: {result.Item2}";
            }
            return RedirectToAction("Admin_Rides");
        }

        public IActionResult EditUser(string id)
        {
            var result = _userService.GetByID(id);
            if (result.Item1 != null)
            {
                TempData["ErrorMessage"] = $"User not found: {result.Item1}";
                return RedirectToAction("Admin_Riders");
            }

            var user = result.Item2;
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Admin_Riders");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var result = user.Edit(user.Name, user.DateOfBirth);
                if (result.Item1)
                {
                    var updateResult = _userService.UpdateUser(user);
                    if (updateResult.Item1)
                    {
                        TempData["SuccessMessage"] = "User updated successfully!";
                        return RedirectToAction("Admin_Riders");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"Failed to update user: {updateResult.Item2}";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to update user: {result.Item2}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid data provided.";
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUser(string id)
        {
            var result = _userService.GetByID(id);
            if (result.Item1 != null)
            {
                TempData["ErrorMessage"] = $"User not found: {result.Item1}";
                return RedirectToAction("Admin_Riders");
            }

            var user = result.Item2;
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Admin_Riders");
            }

            var deleteResult = user.Delete();
            if (deleteResult.Item1)
            {
                var updateResult = _userService.UpdateUser(user);
                if (updateResult.Item1)
                {
                    TempData["SuccessMessage"] = "User deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete user: {updateResult.Item2}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to delete user: {deleteResult.Item2}";
            }

            return RedirectToAction("Admin_Riders");
        }

        [HttpPost]
        public IActionResult AddAdmin(string name, string email, string phoneNumber, DateTime dateOfBirth, Gender gender)
        {
            try
            {
                // Create a new admin user
                var admin = new Admin(name, gender, dateOfBirth)
                {
                    Email = email,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                // For now, we'll store in TempData as a simple list
                // In a real application, you'd save to database
                var admins = TempData["Admins"] as List<Admin> ?? new List<Admin>();
                admins.Add(admin);
                TempData["Admins"] = admins;
                TempData["SuccessMessage"] = "Admin added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to add admin: {ex.Message}";
            }

            return RedirectToAction("Admin_Admines");
        }

        [HttpPost]
        public IActionResult EditAdmin(string id, string name, string email, string phoneNumber, DateTime dateOfBirth, Gender gender)
        {
            try
            {
                // For now, we'll update from TempData
                // In a real application, you'd update in database
                var admins = TempData["Admins"] as List<Admin> ?? new List<Admin>();
                var admin = admins.FirstOrDefault(a => a.Id == id);
                
                if (admin != null)
                {
                    admin.Name = name;
                    admin.Email = email;
                    admin.PhoneNumber = phoneNumber;
                    admin.DateOfBirth = dateOfBirth;
                    admin.Gender = gender;
                    
                    TempData["Admins"] = admins;
                    TempData["SuccessMessage"] = "Admin updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Admin not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to update admin: {ex.Message}";
            }

            return RedirectToAction("Admin_Admines");
        }

        [HttpPost]
        public IActionResult DeleteAdmin(string id)
        {
            try
            {
                // For now, we'll remove from TempData
                // In a real application, you'd delete from database
                var admins = TempData["Admins"] as List<Admin> ?? new List<Admin>();
                var admin = admins.FirstOrDefault(a => a.Id == id);
                
                if (admin != null)
                {
                    admins.Remove(admin);
                    TempData["Admins"] = admins;
                    TempData["SuccessMessage"] = "Admin deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Admin not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete admin: {ex.Message}";
            }

            return RedirectToAction("Admin_Admines");
        }
    }
}
