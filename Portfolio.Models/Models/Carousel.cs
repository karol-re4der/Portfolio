using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Models.Models
{
    public class Carousel
    {
        [Key]
        public int Id { get; set; }
        public int Order { get; set; }
        public bool IsHidden { get; set; }

        public Photo ?Photo  { get; set; }
        public int ?PhotoId { get; set; }
    }
}
