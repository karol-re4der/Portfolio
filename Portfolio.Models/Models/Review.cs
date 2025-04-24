using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Models.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

		[DisplayName("Treść Recenzji")]
		public string? ReviewText { get; set; }
		[DisplayName("Autor Recenzji")]
		public string ReviewAuthor { get; set; }

		[ValidateNever]
		public int ?ReviewPhotoId { get; set; }
		[ValidateNever]
		public Photo ReviewPhoto { get; set; }
    }
}
