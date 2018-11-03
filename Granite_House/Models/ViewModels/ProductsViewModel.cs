using System.Collections.Generic;

namespace Granite_House.Models.ViewModels
{
    public class ProductsViewModel
    {
        public Products Products { get; set; }
        public IEnumerable<ProductTypes> ProductTypes { get; set; }
        public IEnumerable<SpecialTags> SpecialTags { get; set; }
    }
}
