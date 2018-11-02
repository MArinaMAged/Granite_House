using System.ComponentModel.DataAnnotations;

namespace Granite_House.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
