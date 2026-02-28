using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Models.Models
{
    public class ResolutionConfig
    {
        [Key]
        public int Id { get; set; }

        public int ShortSide { get; set; }
    }
}
