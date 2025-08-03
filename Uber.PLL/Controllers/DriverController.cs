using Microsoft.AspNetCore.Mvc;

namespace Uber.PLL.Controllers
{
    public class DriverController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
