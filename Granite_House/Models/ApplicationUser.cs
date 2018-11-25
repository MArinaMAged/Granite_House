using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Granite_House.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name="Sales Person")]
        public string Name { get; set; }

        [NotMapped]
        public bool isSuperAdmin { get; set; }
    }
}
