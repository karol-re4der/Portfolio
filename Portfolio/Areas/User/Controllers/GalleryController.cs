using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.DataAccess.Data;
using Portfolio.Models;
using Portfolio.Models.Models;
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
        private readonly SignInManager<IdentityUser> _signInManager;


        public GalleryController(ILogger<GalleryController> logger, ApplicationDbContext db, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _db = db;
            _signInManager = signInManager;
        }

        public IActionResult Section(string section)
        {
            if (string.IsNullOrWhiteSpace(section))
            {
                return RedirectToAction("Index", "Home", new { area = "User" });
            }

            SectionViewModel viewModel = new SectionViewModel
            {
                Section = _db.Section.Include("SectionCover").Include("SectionCover.PhotoVersions").Include("Albums").Include("Albums.CoverPhoto").Include("Albums.CoverPhoto.PhotoVersions").Include("Albums.CoverPhoto.PhotoPositions").Include("Albums.CoverPhoto.PhotoPositions.PhotoPositionType").FirstOrDefault(x => x.UrlRef.Equals(section))
            };

            viewModel.Section.Albums = viewModel.Section.Albums.Where(x => _signInManager.IsSignedIn(User) || !x.IsHidden).OrderByDescending(x => x.AlbumDateTime).ToList();

            if (viewModel.Section == null)
            {
                return NotFound();
            }

            if (viewModel.Section == null || viewModel.Section.Albums == null || viewModel.Section.Albums.Count() == 0)
            {

            }
                return View(viewModel);
        }

        public IActionResult Album(string album, string returnRef = "")
        {
            AlbumViewModel viewModel = new AlbumViewModel();

            if (album.IsNullOrEmpty() && _signInManager.IsSignedIn(User))
            {
                Portfolio.Models.Models.Album dummyAlbum = new Portfolio.Models.Models.Album();

                List<int> photosInUse = new List<int>();
                photosInUse.AddRange(_db.Album.Select(x => x.CoverPhotoId == null ? -1 : (int)x.CoverPhotoId));
                photosInUse.AddRange(_db.Section.Select(x => x.SectionCoverId == null ? -1 : (int)x.SectionCoverId));
                photosInUse.AddRange(_db.Review.Select(x => x.ReviewPhotoId == null ? -1 : (int)x.ReviewPhotoId));
                photosInUse.AddRange(_db.Carousel.Select(x => x.PhotoId == null ? -1 : (int)x.PhotoId));
                photosInUse.AddRange(_db.AlbumPhoto.Select(x => x.PhotoId));

                photosInUse = photosInUse.Where(x => x > 0).Distinct().ToList();


                List<Photo> photos = _db.Photo.Where(x => !photosInUse.Contains(x.Id)).Include("PhotoVersions").ToList();
                dummyAlbum.Photos = photos;
                dummyAlbum.AlbumName = "Unused photos";

                viewModel.Album = dummyAlbum;
            }
            else
            {
                viewModel.Album = _db.Album.Where(x => _signInManager.IsSignedIn(User) || !x.IsHidden).Include("CoverPhoto").Include("CoverPhoto.PhotoVersions").Include("Photos").Include("Photos.PhotoVersions").FirstOrDefault(x => x.UrlRef.Equals(album));
            }

            viewModel.ReturnRef = returnRef;

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
