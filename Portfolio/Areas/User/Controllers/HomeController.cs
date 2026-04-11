using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.DataAccess.Data;
using Portfolio.Models.Models.ViewModels;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Helpers;

namespace Portfolio.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _db = db;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            Random rand = new Random();
            model.Sections = _db.Section.Where(x => _signInManager.IsSignedIn(User) || !x.IsHidden).Include("SectionCover").Include("SectionCover.PhotoVersions").Include("SectionCover.PhotoPositions").Include("SectionCover.PhotoPositions.PhotoPositionType").OrderBy(x => x.Order).ToList();
            model.Reviews = _db.Review.Include("ReviewPhoto").Include("ReviewPhoto.PhotoVersions").Include("ReviewPhoto.PhotoPositions").Include("ReviewPhoto.PhotoPositions.PhotoPositionType").ToList().OrderBy(x=> rand.NextDouble()).Take(3).ToList();
            model.Carousels = _db.Carousel.Include("Photo").Include("Photo.PhotoVersions").ToList().OrderBy(x => x.Order).Take(3).ToList();

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
        public IActionResult Error(int? statusCode = null)
        {
            ErrorViewModel vm = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, StatusCode = statusCode };
            return View(vm);
        }
    }
}
