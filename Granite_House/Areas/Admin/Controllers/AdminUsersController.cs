using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Granite_House.Data;
using Microsoft.AspNetCore.Mvc;

namespace Granite_House.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminUsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.ApplicationUsers.ToList());
        }
    }
}