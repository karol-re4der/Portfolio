using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models.Models
{
    public class AlbumPhoto
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
