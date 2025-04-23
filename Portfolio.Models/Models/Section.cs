using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Required]
		[DisplayName("Nazwa URL")]
		public string UrlRef { get; set; }

		[DisplayName("Nazwa Sekcji")]
		public string SectionName { get; set; } = "";
		[DisplayName("Opis Sekcji")]
		public string? SectionDescription { get; set; } = "";
		[DisplayName("Ukryta")]
		public bool IsHidden { get; set; }
        public int? Order { get; set; }

		[ValidateNever]
		public int? SectionCoverId { get; set; }
		[ValidateNever]
		[DisplayName("Okładka Albumu")]
		public Photo SectionCover { get; set; }

        [ValidateNever]
        public List<Album> Albums { get; set; }
		[ValidateNever]
		public List<AlbumSection> AlbumsSections { get; set; }
    }
}
