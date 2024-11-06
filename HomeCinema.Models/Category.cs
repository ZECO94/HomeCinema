using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1,100)]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
        // Relation between Category & Movie
        public IEnumerable<Movie>? Movies { get; set; }
    }
}
