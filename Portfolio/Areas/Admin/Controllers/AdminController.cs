using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfolio.Areas.User.Controllers;
using Portfolio.DataAccess.Data;
using Portfolio.Models.Models;
using Portfolio.Models.Models.ViewModels;
using Portfolio.Utility.Utility.Image;
using System.Diagnostics;

namespace Portfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ILogger<AdminController> logger, ApplicationDbContext db, SignInManager<IdentityUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
			_webHostEnvironment = webHostEnvironment;
			_signInManager = signInManager;
			_logger = logger;
            _db = db;
        }

        #region Section
        [HttpGet]
        public IActionResult UpsertSection()
        {
            UpsertSectionViewModel viewModel = new UpsertSectionViewModel()
            {
                Section = new Section()
            };
            return View(viewModel);
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
						Album album = viewModel.Album;

						if (viewModel.PreviewImage != null)
                        {
							string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.PreviewImage);

							Photo newPhoto = new Photo()
                            {
                                Width = ImageUtility.ALBUM_COVER_WIDTH,
                                Height = ImageUtility.ALBUM_COVER_HEIGHT,
                                Path = photoPath,
                                IsHidden = false,
                                NSFW = false
                            };

                            _db.Photo.Update(newPhoto);
							_db.SaveChanges();
							album.CoverPhotoId = newPhoto.Id;

						}

						_db.Album.Update(album);
						_db.SaveChanges();

						_db.AlbumSection.RemoveRange(_db.AlbumSection.Where(x => x.AlbumId == album.Id));

                        _db.AlbumSection.AddRange(viewModel.SectionIdSelected.Select(x => new AlbumSection() { AlbumId = album.Id, SectionId = x }));
						_db.SaveChanges();

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

				return RedirectToAction("Album", "Gallery", new { area="User", album = viewModel.Album.UrlRef });
			}
			return NotFound();
		}

		[HttpGet]
        public IActionResult UpsertAlbum(int albumId = 0)
        {
            UpsertAlbumViewModel viewModel = new UpsertAlbumViewModel();
            Album existingAlbum;

            if (albumId > 0)
            {
                existingAlbum = _db.Album.Include("CoverPhoto").FirstOrDefault(x => x.Id == albumId);
                if (existingAlbum == null)
                {
                    return NotFound();
                }
                else
                {
                    List<int> sectionIdSelected = _db.AlbumSection.Where(x => x.AlbumId == albumId).Select(x => x.SectionId).ToList();

                    viewModel.Album = existingAlbum;
                    viewModel.SectionsAvailable = _db.Section.Select(x => new SelectListItem() { Text = x.SectionName, Value = x.Id.ToString(), Selected = sectionIdSelected.Contains(x.Id) }).ToList();
                }
            }
            else
            {
                viewModel.Album = new Album();
                viewModel.Album.AlbumDateTime = DateTime.Now;
                viewModel.SectionsAvailable = _db.Section.Select(x => new SelectListItem() { Text = x.SectionName, Value = x.Id.ToString(), Selected = false }).ToList();
            }

            return View(viewModel);
        }

		[HttpGet]
		public IActionResult RemoveAlbum(int albumId, string returnRef)
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

            return RedirectToAction("Section", "Gallery", new { area = "User", section=returnRef });
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
            _db.AlbumPhoto.RemoveRange(_db.AlbumPhoto.Where(x => x.AlbumId == albumId && x.PhotoId == photoId));
            _db.SaveChanges();

            return RedirectToAction("Album", "Gallery", new { area = "User", album = returnRef });
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
						Album album = viewModel.Album;

						if (viewModel.PhotosUploaded != null && viewModel.PhotosUploaded.Count()>0)
						{
							foreach (FormFile file in viewModel.PhotosUploaded)
							{
								string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, file);

								Photo newPhoto = new Photo()
								{
									Width = ImageUtility.ALBUM_COVER_WIDTH,
									Height = ImageUtility.ALBUM_COVER_HEIGHT,
									Path = photoPath,
									IsHidden = false,
									NSFW = false
								};

								_db.Photo.Update(newPhoto);
								_db.SaveChanges();
								_db.AlbumPhoto.Add(new AlbumPhoto() { AlbumId = album.Id, PhotoId = newPhoto.Id });
								_db.SaveChanges();
							}
						}
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
