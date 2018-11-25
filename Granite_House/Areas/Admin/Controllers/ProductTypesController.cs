using System.Linq;
using System.Threading.Tasks;
using Granite_House.Data;
using Granite_House.Models;
using Granite_House.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Granite_House.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            return View(_db.ProductTypes.ToList());
        }

        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if(ModelState.IsValid)
            {
                _db.Add(productTypes);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }

        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            ProductTypes productType = await _db.ProductTypes.FindAsync(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }

        //POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductTypes productTypes)
        {
            if (id != productTypes.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            ProductTypes productType = await _db.ProductTypes.FindAsync(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }

        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            ProductTypes productType = await _db.ProductTypes.FindAsync(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }

        //POST Delete Action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ProductTypes productTypes = await _db.ProductTypes.FindAsync(id);
            if (productTypes == null)
                return NotFound();
            _db.ProductTypes.Remove(productTypes);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}