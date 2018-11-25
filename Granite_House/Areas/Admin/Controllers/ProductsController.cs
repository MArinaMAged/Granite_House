using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Granite_House.Data;
using Granite_House.Models;
using Granite_House.Models.ViewModels;
using Granite_House.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Granite_House.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
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
                string uploads = Path.Combine(webRootPath, SD.ImageFolder+@"\"+SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg");
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".jpg";
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET Products Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();

            return View(ProductsVM);
        }

        //POST Products Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if(ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                IFormFileCollection files = HttpContext.Request.Form.Files;

                Products productFromDb = _db.Products.Where(p => p.Id == ProductsVM.Products.Id).FirstOrDefault();

                if(files != null && files.Count>0 && files[0] != null && files[0].Length>0)
                {
                    //if user uploads a new image
                    string uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, ProductsVM.Products.Id + extension_old)))
                        System.IO.File.Delete(Path.Combine(uploads, ProductsVM.Products.Id + extension_old));

                    using (var fileStream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    ProductsVM.Products.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension_new;
                }

                productFromDb.Name = ProductsVM.Products.Name;
                productFromDb.Price = ProductsVM.Products.Price;
                productFromDb.Available = ProductsVM.Products.Available;
                productFromDb.ProductTypeId = ProductsVM.Products.ProductTypeId;
                productFromDb.SpecialTagsId = ProductsVM.Products.SpecialTagsId;
                productFromDb.ShadeColor = ProductsVM.Products.ShadeColor;
                if (ProductsVM.Products.Image != null)
                    productFromDb.Image = ProductsVM.Products.Image;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ProductsVM);
        }

        //GET Products Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();

            return View(ProductsVM);
        }

        //GET Products Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductsVM.Products == null)
                return NotFound();

            return View(ProductsVM);
        }

        //POST Products Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Products products =  await _db.Products.FindAsync(id);
            if (products == null)
                return NotFound();

            string uploads = Path.Combine(webRootPath, SD.ImageFolder);
            string extension = Path.GetExtension(products.Image);
            if (System.IO.File.Exists(Path.Combine(uploads, products.Id + extension)))
                System.IO.File.Delete(Path.Combine(uploads, products.Id + extension));

            _db.Products.Remove(products);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}