using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Granite_House.Models;
using Granite_House.Data;
using Microsoft.EntityFrameworkCore;

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
