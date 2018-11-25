using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Granite_House.Data;
using Granite_House.Models.ViewModels;
using Granite_House.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Granite_House.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.AdminEndUser + ","+SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AppointmentsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            AppointmentViewModel appointmentVM = new AppointmentViewModel()
            {
                Appointments = new List<Models.Appointments>()
            };

            appointmentVM.Appointments = _db.Appointments.Include(a => a.SalesPerson).ToList();
            if (User.IsInRole(SD.AdminEndUser))
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.SalesPersonId == claim.Value).ToList();

            return View(appointmentVM);
        }
    }
}