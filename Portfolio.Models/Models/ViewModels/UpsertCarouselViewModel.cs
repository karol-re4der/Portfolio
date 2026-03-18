using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Models.Models.ViewModels
{
    public class UpsertCarouselViewModel
    {
        [ValidateNever]
        public Carousel CarouselLeft { get; set; }
        public Carousel CarouselMid { get; set; }
        public Carousel CarouselRight { get; set; }


        [ValidateNever]
        public IFormFile? CarouselPhotoLeft { get; set; }
        public IFormFile? CarouselPhotoMid { get; set; }
        public IFormFile? CarouselPhotoRight { get; set; }


    }
}
