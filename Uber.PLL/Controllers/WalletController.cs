using Microsoft.AspNetCore.Mvc;

namespace Uber.PLL.Controllers
{
    public class WalletController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
