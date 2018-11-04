using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Granite_House.Models;
using Granite_House.Data;
using Microsoft.EntityFrameworkCore;
using Granite_House.Extensions;

namespace Granite_House.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
            

        public async Task<IActionResult> Index()
        {
            IEnumerable<Products> productList = await _db.Products.Include(p => p.ProductTypes).Include(p => p.SpecialTags).ToListAsync();
            return View(productList);
        }

        // GET Details
        public async Task<IActionResult> Details(int id)
        {
            Products product = await _db.Products.Include(p => p.ProductTypes).Include(p => p.SpecialTags).Where(p => p.Id == id).FirstOrDefaultAsync();
            return View(product);
        }

        // POST Details
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPost(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (lstShoppingCart == null)
                lstShoppingCart = new List<int>();
            lstShoppingCart.Add(id);
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (lstShoppingCart.Count > 0 && lstShoppingCart.Contains(id))
                lstShoppingCart.Remove(id);

            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
