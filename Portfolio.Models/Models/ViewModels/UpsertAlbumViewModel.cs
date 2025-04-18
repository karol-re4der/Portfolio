using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Models.ViewModels
{
    public class UpsertAlbumViewModel
    {
        public Album Album { get; set; }

        [ValidateNever]
        public IFormFile? PreviewImage { get; set; }
        [ValidateNever]
        public List<SelectListItem> SectionsAvailable { get; set; } = new List<SelectListItem>();
		[Required(ErrorMessage = "Wybór sekcji jest wymagany")]
		public List<int> SectionIdSelected { get; set; } = new List<int>();
	}
}
