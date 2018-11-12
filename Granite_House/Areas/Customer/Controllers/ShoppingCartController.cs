using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Granite_House.Data;
using Granite_House.Extensions;
using Granite_House.Models;
using Granite_House.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Granite_House.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Models.Products>()
            };
        }

        //GET Index Shopping Cart
        public IActionResult Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingCart!=null && lstShoppingCart.Count>0)
            {
                foreach(int cartItem in lstShoppingCart)
                {
                    Products products = _db.Products
                        .Include(p=>p.SpecialTags)
                        .Include(p=>p.ProductTypes)
                        .Where(p => p.Id == cartItem).FirstOrDefault();
                    ShoppingCartVM.Products.Add(products);
                }
            }
            return View(ShoppingCartVM);
        }
    }
}