using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        public bool NSFW { get; set; }
        public bool IsHidden { get; set; }

        public List<Album> Albums { get; set; }
        public List<AlbumPhoto> AlbumsPhotos { get; set; }
        public List<Album> AlbumCovers { get; set; }
        public List<Section> SectionCovers { get; set; }
        public List<PhotoVersion> PhotoVersions{ get; set; }
        public List<PhotoPosition> PhotoPositions { get; set; }
    }
}
