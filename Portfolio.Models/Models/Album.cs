using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

		[Required(ErrorMessage = "Pole jest wymagane")]
		[DisplayName("Nazwa URL")]
		public string UrlRef { get; set; }

		[Required(ErrorMessage = "Pole jest wymagane")]

		[DisplayName("Nazwa Albumu")]
        public string AlbumName { get; set; } = "";
		[DisplayName("Opis Albumu")]
		public string? AlbumDescription { get; set; } = "";
		[DisplayName("Data Stworzenia")]
		public DateTime AlbumDateTime { get; set; }
		[DisplayName("Ukryty")]
		public bool IsHidden { get; set; }
		[DisplayName("NSFW")]
		public bool IsNSFW { get; set; }

		[ValidateNever]
		public int? CoverPhotoId { get; set; }
		[ValidateNever]
		[DisplayName("Okładka Albumu")]
		public Photo CoverPhoto { get; set; }

        [ValidateNever]
        public List<Section> Sections { get; set; }
		[ValidateNever]
		public List<AlbumSection> AlbumsSections { get; set; }
		[ValidateNever]
		public List<Photo> Photos { get; set; }
		[ValidateNever]
		public List<AlbumPhoto> AlbumsPhotos { get; set; }
    }
}
