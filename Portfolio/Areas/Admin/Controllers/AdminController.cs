using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfolio.Areas.Admin.Services;
using Portfolio.Areas.Admin.Services.Interfaces;
using Portfolio.Areas.User.Controllers;
using Portfolio.DataAccess.Data;
using Portfolio.Migrations;
using Portfolio.Models.Models;
using Portfolio.Models.Models.ViewModels;
using Portfolio.Utility.Utility.Image;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Portfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAdminService _adminService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ILogger<AdminController> logger, ApplicationDbContext db, SignInManager<IdentityUser> signInManager, IWebHostEnvironment webHostEnvironment, IAdminService adminService)
        {
			_webHostEnvironment = webHostEnvironment;
			_signInManager = signInManager;
			_logger = logger;
            _db = db;
            _adminService = adminService;
        }

        #region Section
        [HttpGet]
        public IActionResult UpsertSection(int sectionId=0)
        {
			UpsertSectionViewModel viewModel = new UpsertSectionViewModel();
			Section existingSection;

			if (sectionId > 0)
			{
				existingSection = _db.Section.Include("SectionCover").Include("SectionCover.PhotoVersions").FirstOrDefault(x => x.Id == sectionId);
				if (existingSection == null)
				{
					return NotFound();
				}
				else
				{
					viewModel.Section = existingSection;
				}
			}
			else
			{
				viewModel.Section = new Section();
			}

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult UpsertSection(UpsertSectionViewModel viewModel)
		{
			if (_signInManager.IsSignedIn(User))
			{
				try
				{
					if (ModelState.IsValid)
					{
                        _adminService.UpsertSection(viewModel);
                    }
					else
					{
						return NotFound();
					}
				}
				catch (Exception e)
				{
					return NotFound();
				}

				return RedirectToAction("Section", "Gallery", new { area = "User", section = viewModel.Section.UrlRef });
			}
			return NotFound();
		}

		[HttpGet]
		public IActionResult RemoveSection(int sectionId)
		{
			if (_signInManager.IsSignedIn(User))
			{
				Section section = _db.Section.FirstOrDefault(x => x.Id == sectionId);
				if (section == null)
				{
					return NotFound();
				}

				_db.AlbumSection.RemoveRange(_db.AlbumSection.Where(x => x.SectionId == sectionId));
				_db.SaveChanges();

				_db.Section.Remove(section);
				_db.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            else
			{
				return NotFound();
			}
		}
		#endregion

		#region Album
		[HttpPost]
        public IActionResult UpsertAlbum(UpsertAlbumViewModel viewModel)
        {
            if (_signInManager.IsSignedIn(User))
			{
				try
				{
					if (ModelState.IsValid)
					{
                        _adminService.UpsertAlbum(viewModel);
					}
					else
					{
						return NotFound();
					}
				}
				catch (Exception e)
				{
					return NotFound();
				}

				return RedirectToAction("Album", "Gallery", new { area="User", album = viewModel.Album.UrlRef, returnRef = "" });
			}
			return NotFound();
		}

		[HttpGet]
		public IActionResult UpsertAlbum(int albumId = 0, int targetSectionId = 0)
        {
            UpsertAlbumViewModel viewModel = new UpsertAlbumViewModel();
            Album existingAlbum;

            if (albumId > 0)
            {
                existingAlbum = _db.Album.Include("CoverPhoto").Include("CoverPhoto.PhotoVersions").FirstOrDefault(x => x.Id == albumId);
                if (existingAlbum == null)
                {
                    return NotFound();
                }
                else
                {
                    List<int> sectionIdSelected = _db.AlbumSection.Where(x => x.AlbumId == albumId).Select(x => x.SectionId).ToList();

                    viewModel.Album = existingAlbum;
                    viewModel.SectionsAvailable = _db.Section.Select(x => new SelectListItem() { Text = x.SectionName, Value = x.Id.ToString(), Selected = sectionIdSelected.Contains(x.Id) }).ToList();
					viewModel.SectionIdSelected = sectionIdSelected;
                }
            }
            else
            {
                viewModel.Album = new Album();
                viewModel.Album.AlbumDateTime = DateTime.Now;
                viewModel.SectionsAvailable = _db.Section.Select(x => new SelectListItem() { Text = x.SectionName, Value = x.Id.ToString(), Selected = x.Id==targetSectionId }).ToList();
				viewModel.SectionIdSelected.Add(targetSectionId);
            }

            return View(viewModel);
        }

		[HttpGet]
		public IActionResult RemoveAlbum(int albumId, string returnRef)
		{
			if (_signInManager.IsSignedIn(User))
			{
				Album album = _db.Album.FirstOrDefault(x => x.Id == albumId);
				if (album == null)
				{
					return NotFound();
				}

				_db.AlbumPhoto.RemoveRange(_db.AlbumPhoto.Where(x => x.AlbumId == albumId));
				_db.SaveChanges();

				_db.AlbumSection.RemoveRange(_db.AlbumSection.Where(x => x.AlbumId == albumId));
				_db.SaveChanges();

				_db.Album.Remove(album);
				_db.SaveChanges();

                return RedirectToAction("Section", "Gallery", new { area = "User", section = returnRef });
            }
			return NotFound();
        }
		#endregion

		#region Photos
		[HttpGet]
		public IActionResult UpsertPhotos(int albumId)
        {
			UpsertPhotosViewModel viewModel = new();

			if (albumId > 0)
			{
				viewModel.Album = _db.Album.Include("Photos").FirstOrDefault(x => x.Id == albumId);
				if (viewModel.Album == null)
				{
					return NotFound();
				}
			}
			else
			{
                return NotFound();
			}

            return View(viewModel);
        }

        [HttpGet]
		public IActionResult RemovePhotoFromAlbum(int photoId, int albumId, string returnRef)
		{
			if (_signInManager.IsSignedIn(User))
			{
				_db.AlbumPhoto.RemoveRange(_db.AlbumPhoto.Where(x => x.AlbumId == albumId && x.PhotoId == photoId));
				_db.SaveChanges();

				return RedirectToAction("Album", "Gallery", new { area = "User", album = returnRef });
			}
			return NotFound();
        }

        [HttpPost]
        public IActionResult UpsertPhotos(UpsertPhotosViewModel viewModel)
        {
            if (_signInManager.IsSignedIn(User))
			{
				try
				{
					if (ModelState.IsValid)
					{
                        _adminService.UpsertPhotos(viewModel);
					}
					else
					{
						return NotFound();
					}
				}
				catch (Exception e)
				{
					return NotFound();
				}

				return RedirectToAction("Album", "Gallery", new { area = "User", album = viewModel.Album.UrlRef });
			}
			return NotFound();
		}
		#endregion

		#region Review
		[HttpPost]
		public IActionResult UpsertReview(UpsertReviewViewModel viewModel)
		{
			if (_signInManager.IsSignedIn(User))
			{
				try
				{
					if (ModelState.IsValid)
					{
                        _adminService.UpsertReview(viewModel);
					}
					else
					{
						return NotFound();
					}
				}
				catch (Exception e)
				{
					return NotFound();
				}

				return RedirectToAction("Index", "Home", new { area = "User"});
			}
			return NotFound();
		}

		[HttpGet]
		public IActionResult UpsertReview(int reviewId = 0)
		{
			UpsertReviewViewModel viewModel = new UpsertReviewViewModel();
			Review existingReview;

			if (reviewId > 0)
			{
				existingReview = _db.Review.Include("ReviewPhoto").Include("ReviewPhoto.PhotoVersions").FirstOrDefault(x => x.Id == reviewId);
				if (existingReview == null)
				{
					return NotFound();
				}
				else
				{
					viewModel.Review = existingReview;
				}
			}
			else
			{
				viewModel.Review = new Review();
			}

			return View(viewModel);
		}

		[HttpGet]
		public IActionResult RemoveReview(int reviewId)
		{
			if (_signInManager.IsSignedIn(User))
			{
				Review review = _db.Review.FirstOrDefault(x => x.Id == reviewId);
				if (review == null)
				{
					return NotFound();
				}

				_db.Review.Remove(review);
				_db.SaveChanges();

				return RedirectToAction("Index", "Home", new { area = "User"});
			}
			return NotFound();
		}
        #endregion

        #region Carousel
        [HttpPost]
        public IActionResult UpsertCarousel(UpsertCarouselViewModel viewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                try
                {
					_adminService.UpsertCarousel(viewModel);
                }
                catch (Exception e)
                {
                    return NotFound();
                }

                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult UpsertCarousel()
        {
            UpsertCarouselViewModel viewModel = new UpsertCarouselViewModel();

            List<Carousel> carousels = _db.Carousel.Include("Photo").Include("Photo.PhotoVersions").OrderBy(x => x.Order).Take(3).ToList();
            if(carousels != null && carousels.Count() > 2)
            {
                viewModel.CarouselLeft = carousels.ElementAt(0);
                viewModel.CarouselMid = carousels.ElementAt(1);
                viewModel.CarouselRight = carousels.ElementAt(2);

                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

        #region Photo

        [HttpGet]
        public IActionResult UpsertPhoto(int photoId)
        {
            UpsertPhotoViewModel viewModel = new UpsertPhotoViewModel();
            Photo existingPhoto;

            if (photoId > 0)
            {
                existingPhoto = _db.Photo.Include("PhotoVersions").FirstOrDefault(x => x.Id == photoId);
                if (existingPhoto == null)
                {
                    return NotFound();
                }
                else
                {
                    viewModel.Photo = existingPhoto;

                    viewModel.SectionCoversIdSelected = _db.Section.Where(x => x.SectionCoverId == photoId).Select(x => x.Id).ToList();
                    viewModel.SectionCoversAvailable = _db.Section.OrderByDescending(x => viewModel.SectionCoversIdSelected.Contains(x.Id)).ThenBy(x => x.SectionName).Select(x=>new SelectListItem(x.SectionName, x.Id.ToString())).ToList();

                    viewModel.AlbumCoversIdSelected = _db.Album.Where(x => x.CoverPhotoId == photoId).Select(x => x.Id).ToList();
                    viewModel.AlbumCoversAvailable = _db.Album.OrderByDescending(x => viewModel.AlbumCoversIdSelected.Contains(x.Id)).ThenBy(x => x.AlbumName).Select(x => new SelectListItem(x.AlbumName, x.Id.ToString())).ToList();

                    viewModel.AlbumsIdSelected = _db.AlbumPhoto.Where(x => x.PhotoId == photoId).Select(x => x.AlbumId).Distinct().ToList();
                    viewModel.AlbumsAvailable = _db.Album.OrderByDescending(x => viewModel.AlbumsIdSelected.Contains(x.Id)).ThenBy(x => x.AlbumName).Select(x => new SelectListItem(x.AlbumName, x.Id.ToString())).ToList();

                    viewModel.ReviewsIdSelected = _db.Review.Where(x => x.ReviewPhotoId == photoId).Select(x => x.Id).ToList();
                    viewModel.ReviewsAvailable = _db.Review.OrderByDescending(x => viewModel.ReviewsIdSelected.Contains(x.Id)).ThenBy(x => x.ReviewAuthor).Select(x => new SelectListItem(x.ReviewAuthor, x.Id.ToString())).ToList();

                    viewModel.CarouselsIdSelected = _db.Carousel.Where(x => x.PhotoId == photoId).Select(x => x.Id).ToList();
                    viewModel.CarouselsAvailable = _db.Carousel.OrderBy(x => x.Id).Select(x => new SelectListItem(x.Id.ToString(), x.Id.ToString())).ToList();

					viewModel.MissingRes = _adminService.FindMissingVersions(existingPhoto);
                }
            }
            else
            {
                //No adding new photos from here
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpsertPhoto(UpsertPhotoViewModel viewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        _adminService.UpsertPhoto(viewModel);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception e)
                {
                    return NotFound();
                }
                return RedirectToAction("Index", "Home", new { area = "User"});
            }
            return NotFound();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
