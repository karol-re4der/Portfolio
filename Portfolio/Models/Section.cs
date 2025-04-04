using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        public string SectionName { get; set; } = "";
        public string SectionDescription { get; set; } = "";
        public bool IsHidden { get; set; }
        public int Order { get; set; }
    }
}
