using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Models.Models
{
    public class PhotoPosition
    {
        [Key]
        public int Id { get; set; }

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Scale { get; set; }

        public int PhotoPositionTypeId { get; set; }
        public PhotoPositionType ?PhotoPositionType { get; set; }

        public int PhotoId { get; set; }
        public Photo? Photo { get; set; }
    }
}
