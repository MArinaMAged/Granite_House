using Granite_House.Models;
using Granite_House.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Data
{
    public class DbInit : IDbInit
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInit(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void Initialize()
        {
            if(_db.Database.GetPendingMigrations().Count()>0)
                _db.Database.Migrate();

            if (_db.Roles.Any(r => r.Name == SD.SuperAdminEndUser))
                return;

            _roleManager.CreateAsync(new IdentityRole(SD.AdminEndUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.SuperAdminEndUser)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "admin@fantics.cloud",
                Email = "admin@fantics.cloud",
                Name = "Adminisztrátor",
                EmailConfirmed = true
            }, "Alpha707#").GetAwaiter().GetResult();

            IdentityUser user = await _db.Users.Where(p => p.Email == "admin@fantics.cloud").FirstOrDefaultAsync();
            await _userManager.AddToRoleAsync(user, SD.SuperAdminEndUser);
        }
    }
}
