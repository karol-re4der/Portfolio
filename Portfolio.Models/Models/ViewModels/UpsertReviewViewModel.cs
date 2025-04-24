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
    public class UpsertReviewViewModel
    {
        [ValidateNever]
        public Review Review { get; set; }

        [ValidateNever]
        public IFormFile? PreviewImage { get; set; }
    }
}
