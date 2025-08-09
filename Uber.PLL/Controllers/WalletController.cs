using Microsoft.AspNetCore.Mvc;
using Uber.DAL.Entities;
using Uber.DAL.DataBase;

namespace Uber.PLL.Controllers
{
    public class WalletController : Controller
    {
        //private readonly UberDBContext _context;

        //public WalletController(UberDBContext context)
        //{
        //    _context = context;
        //}

        //public IActionResult Index()
        //{
        //    var wallets = _context.Wallets.ToList();
        //    return View(wallets);
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(Wallet wallet)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Wallets.Add(wallet);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(wallet);
        //}

        //public IActionResult Edit(int id)
        //{
        //    var wallet = _context.Wallets.Find(id);
        //    if (wallet == null) return NotFound();
        //    return View(wallet);
        //}

        //[HttpPost]
        //public IActionResult Edit(Wallet wallet)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Wallets.Update(wallet);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(wallet);
        //}

        //public IActionResult Delete(int id)
        //{
        //    var wallet = _context.Wallets.Find(id);
        //    if (wallet == null) return NotFound();

        //    _context.Wallets.Remove(wallet);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
