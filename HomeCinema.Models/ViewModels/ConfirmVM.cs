using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Models.ViewModels
{
    public class ConfirmVM
    {
        public Order Order { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
