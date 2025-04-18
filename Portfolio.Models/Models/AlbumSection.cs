namespace Portfolio.Models.Models
{
    public class AlbumSection
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
