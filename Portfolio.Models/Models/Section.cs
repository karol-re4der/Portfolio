using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UrlRef { get; set; }

        public string SectionName { get; set; } = "";
        public string SectionDescription { get; set; } = "";
        public bool IsHidden { get; set; }
        public int Order { get; set; }

        public int SectionCoverId { get; set; }
        [ForeignKey("SectionCoverId")]
        public Photo SectionCover { get; set; }

        public List<Album> Albums { get; set; }
        public List<AlbumSection> AlbumsSections { get; set; }
    }
}
