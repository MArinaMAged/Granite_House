using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Products> Products { get; set; }
        public Appointments Appointments { get; set; }
    }
}
