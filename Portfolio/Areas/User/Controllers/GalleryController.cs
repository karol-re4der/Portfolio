using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.DataAccess.Data;
using Portfolio.Models;
using Portfolio.Models.Models.ViewModels;
using System.Diagnostics;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

namespace Portfolio.Areas.User.Controllers
{
    [Area("User")]
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
            SectionViewModel viewModel = new SectionViewModel
            {
                Section = _db.Section.Include("SectionCover").Include("Albums").Include("Albums.CoverPhoto").FirstOrDefault(x => x.UrlRef.Equals(section))
            };

            if (viewModel.Section == null)
            {
                return NotFound();
            }

            if (viewModel.Section == null || viewModel.Section.Albums == null || viewModel.Section.Albums.Count() == 0)
            {

            }
                return View(viewModel);
        }

        public IActionResult Album(string album)
        {
            AlbumViewModel viewModel = new AlbumViewModel
            {
                Album = _db.Album.Include("CoverPhoto").Include("Photos").FirstOrDefault(x => x.UrlRef.Equals(album))
            };

            if (viewModel.Album == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
