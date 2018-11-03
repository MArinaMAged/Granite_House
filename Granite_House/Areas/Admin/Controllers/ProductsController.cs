using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Granite_House.Data;
using Granite_House.Models;
using Granite_House.Models.ViewModels;
using Granite_House.Utility;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Granite_House.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        [BindProperty]
        public ProductsViewModel ProductsVM { get; set; }

        public ProductsController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            ProductsVM = new ProductsViewModel()
            {
                ProductTypes = _db.ProductTypes.OrderBy(p=>p.Name).ToList(),
                SpecialTags = _db.SpecialTags.OrderBy(p => p.Name).ToList(),
                Products = new Products()
            };
        }

        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(pt => pt.ProductTypes).Include(st => st.SpecialTags);
            return View(await products.ToListAsync());
        }

        //GET Products Create
        public IActionResult Create()
        {
            return View(ProductsVM);
        }

        //POST Products Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid)
                return View(ProductsVM);
            _db.Products.Add(ProductsVM.Products);
            await _db.SaveChangesAsync();
            Products productsFromDb = _db.Products.Find(ProductsVM.Products.Id);

            //Image being saved
            string webRootPath = _hostingEnvironment.WebRootPath;
            IFormFileCollection files = HttpContext.Request.Form.Files;
            if (files.Count!=0)
            {
                //files has been uploaded
                string uploads = Path.Combine(webRootPath, SD.ImageFolder);
                string extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension;
            }
            else
            {
                //when user doesn't upload image
                string uploads = Path.Combine(webRootPath, SD.ImageFolder+SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg");
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg";
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}