using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [Range(1,500)]
        public int Count { get; set; }
        // Relations with tbls
        [ForeignKey("Movie")]
        public int MovieId { get; set;}
        [ValidateNever]
        public Movie Movie { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set;}
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public Order? Order { get; set; }
    }
}
