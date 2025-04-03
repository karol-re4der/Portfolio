using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
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
    }
}
