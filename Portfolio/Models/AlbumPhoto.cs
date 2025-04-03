using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    public class AlbumPhoto
    {
        [Key]
        public int Id { get; set; }

        public int Order { get; set; }

        [ForeignKey("Album")]
        public int AlbumId { get; set; }

        [ForeignKey("Photo")]
        public int SectionId { get; set; }
    }
}
