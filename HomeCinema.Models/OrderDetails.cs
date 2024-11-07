using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        // Relation With Order & Movie
        [ForeignKey("Orer")]
        public int OrderId { get; set; }
        [ValidateNever]
        public Order Order { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        [ValidateNever]
        public Movie Movie { get; set; }

    }
}
