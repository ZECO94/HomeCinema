using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [DisplayName("Actor Name")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Birth Day")]
        public DateTime BirthDate { get; set; }
        [NotMapped]
        public int Age { get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                return age;
            } 
        } 
        public string Gender { get; set; }
        public string ImgUrl { get; set; }
        //Relation between Actor & Movie
        public IEnumerable<Movie>? Movies { get; set;}
    }
}
