using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uber.DAL.DataBase;
using Uber.DAL.Entities;
using System.Threading.Tasks;

namespace Uber.PLL.Controllers
{
    public class AdminController : Controller
    {
        private readonly UberDBContext _context;

        public AdminController(UberDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var riders = await _context.Users.ToListAsync();
            var drivers = await _context.Drivers.ToListAsync();
            ViewBag.Riders = riders;
            ViewBag.Drivers = drivers;
            return View();
        }

        // --- Users ---
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(user);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        // --- Drivers ---
        public async Task<IActionResult> EditDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();
            return View(driver);
        }

        [HttpPost]
        public async Task<IActionResult> EditDriver(Driver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Update(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(driver);
        }

        public async Task<IActionResult> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }
    }
}
