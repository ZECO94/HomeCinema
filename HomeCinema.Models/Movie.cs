using HomeCinema.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImgUrl { get; set; }
        public int Rate { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public MovieStatus Status { get; set; }
        // Relation between Category & Movie 
        [ForeignKey("Category")]
        public int CatId { get; set; }
        public Category? Category { get; set;}
        //Relation between Actor & Movie
        public IEnumerable<Actor>? Actors { get; set; }

    }
}
