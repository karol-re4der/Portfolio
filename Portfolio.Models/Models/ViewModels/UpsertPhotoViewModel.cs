using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
namespace Portfolio.Models.Models.ViewModels
{
    public class UpsertPhotoViewModel
    {
        public Photo Photo { get; set; }

        [ValidateNever]
        public List<SelectListItem> SectionCoversAvailable { get; set; } = new List<SelectListItem>();
        [ValidateNever]
        public List<int> SectionCoversIdSelected { get; set; } = new List<int>();

        [ValidateNever]
        public List<SelectListItem> AlbumCoversAvailable { get; set; } = new List<SelectListItem>();
        [ValidateNever]
        public List<int> AlbumCoversIdSelected { get; set; } = new List<int>();

        [ValidateNever]
        public List<SelectListItem> AlbumsAvailable { get; set; } = new List<SelectListItem>();
        [ValidateNever]
        public List<int> AlbumsIdSelected { get; set; } = new List<int>();

        [ValidateNever]
        public List<SelectListItem> ReviewsAvailable { get; set; } = new List<SelectListItem>();
        [ValidateNever]
        public List<int> ReviewsIdSelected { get; set; } = new List<int>();

        [ValidateNever]
        public List<SelectListItem> CarouselsAvailable { get; set; } = new List<SelectListItem>();
        [ValidateNever]
        public List<int> CarouselsIdSelected { get; set; } = new List<int>();

        [ValidateNever]
        public List<int> MissingRes { get; set; } = new List<int>();
        public bool AddMissingRes { get; set; } = false;

    }

}

