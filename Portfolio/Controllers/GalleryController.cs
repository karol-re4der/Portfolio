using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;
using Portfolio.Models.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace Portfolio.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly ApplicationDbContext _db;

        public GalleryController(ILogger<GalleryController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Section(string section)
        {
            SectionViewModel viewModel = new SectionViewModel{
                Section = _db.Section.Include("Albums").Include("Albums.CoverPhoto").FirstOrDefault(x => x.SectionName.Equals(section))
            };
            
            if (viewModel.Section == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        public IActionResult Album(string album)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
