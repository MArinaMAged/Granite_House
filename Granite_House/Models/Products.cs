using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Models
{
    public class Products
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool Available { get; set; }

        public string Image { get; set; }

        public string ShadeColor { get; set; }

        [Display(Name="Product type")]
        public int ProductTypeId { get; set; }

        [Display(Name = "Product type")]
        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes ProductTypes { get; set; }

        [Display(Name = "Special tag")]
        public int SpecialTagsId { get; set; }

        [Display(Name = "Special tag")]
        [ForeignKey("SpecialTagsId")]
        public virtual SpecialTags SpecialTags { get; set; }

    }
}
