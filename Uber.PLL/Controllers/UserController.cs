using Microsoft.AspNetCore.Mvc;

namespace Uber.PLL.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
