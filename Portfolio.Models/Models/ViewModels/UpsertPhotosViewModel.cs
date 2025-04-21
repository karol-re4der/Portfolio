using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Models.Models.ViewModels
{
    public class UpsertPhotosViewModel
    {
        [ValidateNever]
        public Album Album { get; set; }

        [DisplayName("Nowe zdjęcia")]
        public List<IFormFile> PhotosUploaded { get; set; } = new List<IFormFile>();
    }
}
