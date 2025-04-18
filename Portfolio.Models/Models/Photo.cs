using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        public string Path { get; set; } = "";
        public bool NSFW { get; set; }
        public bool IsHidden { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public List<Album> Albums { get; set; }
        public List<AlbumPhoto> AlbumsPhotos { get; set; }

        public List<Album> AlbumCovers { get; set; }
    }
}
