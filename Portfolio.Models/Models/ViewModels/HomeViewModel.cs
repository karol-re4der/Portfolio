using Portfolio.Models.Models;

namespace Portfolio.Models.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Section> Sections { get; set; } = new List<Section>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Carousel> Carousels { get; set; } = new List<Carousel>();

    }
}
