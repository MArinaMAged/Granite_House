﻿using System.Linq;
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
    public class SpecialTagsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SpecialTagsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }

        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTags specialTags)
        {
            if (ModelState.IsValid)
            {
                if (_db.SpecialTags.Count() > 0)
                    specialTags.Id = _db.SpecialTags.Max(p => p.Id) + 1;
                else
                    specialTags.Id = 1;
                _db.Add(specialTags);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(specialTags);
        }

        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            SpecialTags specialTag = await _db.SpecialTags.FindAsync(id);
            if (specialTag == null)
                return NotFound();
            return View(specialTag);
        }

        //POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpecialTags specialTag)
        {
            if (id != specialTag.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _db.Update(specialTag);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(specialTag);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            SpecialTags specialTag = await _db.SpecialTags.FindAsync(id);
            if (specialTag == null)
                return NotFound();
            return View(specialTag);
        }

        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            SpecialTags specialTag = await _db.SpecialTags.FindAsync(id);
            if (specialTag == null)
                return NotFound();
            return View(specialTag);
        }

        //POST Delete Action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SpecialTags specialTag = await _db.SpecialTags.FindAsync(id);
            if (specialTag == null)
                return NotFound();
            _db.SpecialTags.Remove(specialTag);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}