using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Portfolio.DataAccess.Data;
using Portfolio.Models.Models.ViewModels;

namespace Portfolio.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            Random rand = new Random();
            model.Sections = _db.Section.Where(x => !x.IsHidden).OrderBy(x => x.Order).Include("SectionCover").Include("SectionCover.PhotoVersions").ToList();
            model.Reviews = _db.Review.Include("ReviewPhoto").Include("ReviewPhoto.PhotoVersions").ToList().OrderBy(x=> rand.NextDouble()).Take(3).ToList();

            return View(model);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return Redirect("/Identity/Account/Login/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
