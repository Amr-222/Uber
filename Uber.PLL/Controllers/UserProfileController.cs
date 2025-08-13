using Microsoft.AspNetCore.Mvc;

namespace Uber.PLL.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
