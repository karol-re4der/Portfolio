using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Models.Models
{
    public class PhotoVersion
    {
        [Key]
        public int Id { get; set; }

        public string Path { get; set; } = "";
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsOriginal { get; set; } = false;

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
