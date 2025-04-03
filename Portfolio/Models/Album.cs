using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        public string AlbumName { get; set; } = "";
        public string AlbumDescription { get; set; } = "";
        public DateTime AlbumDateTime { get; set; }
        public bool IsHidden { get; set; }

        [ForeignKey("Photo")]
        public int CoverPhotoId { get; set; }
    }
}
