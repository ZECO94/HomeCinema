using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Cart> carts {  get; set; }
        public Order Order { get; set; }
        
    }
}
