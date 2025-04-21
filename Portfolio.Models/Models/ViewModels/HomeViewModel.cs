using Portfolio.Models.Models;

namespace Portfolio.Models.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Section> Sections { get; set; } = new List<Section>();
        public List<Review> Reviews { get; set; }
    }
}
