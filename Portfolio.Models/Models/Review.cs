using System;
using System.Collections.Generic;
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

        public string? ReviewText { get; set; }
        public string? ReviewAuthor { get; set; }

        public int ?ReviewPhotoId { get; set; }
        public Photo ReviewPhoto { get; set; }
    }
}
