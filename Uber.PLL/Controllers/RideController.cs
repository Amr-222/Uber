using Microsoft.AspNetCore.Mvc;

namespace Uber.PLL.Controllers
{
    public class RideController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
