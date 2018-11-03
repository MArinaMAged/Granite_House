using System.Linq;
using System.Threading.Tasks;
using Granite_House.Data;
using Granite_House.Models;
using Granite_House.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Granite_House.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ProductsViewModel ProductsVM { get; set; }

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
            ProductsVM = new ProductsViewModel()
            {
                ProductTypes = _db.ProductTypes.ToList(),
                SpecialTags = _db.SpecialTags.ToList(),
                Products = new Products()
            };
        }

        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(pt => pt.ProductTypes).Include(st => st.SpecialTags);
            return View(await products.ToListAsync());
        }
    }
}