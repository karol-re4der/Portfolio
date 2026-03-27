using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating.CodeCompilation;
using NuGet.ContentModel;
using Portfolio.Areas.Admin.Controllers;
using Portfolio.Areas.Admin.Services.Interfaces;
using Portfolio.DataAccess.Data;
using Portfolio.Models.Models;
using Portfolio.Models.Models.ViewModels;
using Portfolio.Utility.Utility.Image;
using System.Drawing;
using System.Linq;

namespace Portfolio.Areas.Admin.Services
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminService(ILogger<AdminController> logger, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Photos
        public void UpsertPhotos(UpsertPhotosViewModel viewModel)
        {
            Album album = viewModel.Album;

            if (viewModel.PhotosUploaded != null && viewModel.PhotosUploaded.Count() > 0)
            {
                foreach (FormFile file in viewModel.PhotosUploaded)
                {
                    Photo newPhoto = CreatePhotoWithVersions(file);
                    _db.AlbumPhoto.Add(new AlbumPhoto() { AlbumId = album.Id, PhotoId = newPhoto.Id });
                    _db.SaveChanges();
                }
            }
        }

        public void UpsertPhoto(UpsertPhotoViewModel viewModel)
        {

            if (viewModel.AddMissingRes)
            {
                Photo ph = _db.Photo.Include("PhotoVersions").Where(x => x.Id == viewModel.Photo.Id).FirstOrDefault();

                foreach (int res in FindMissingVersions(ph))
                {
                    CreatePhotoVersion(ph, res);
                }
            }

            //Update section covers
            foreach (Section sect in _db.Section)
            {
                if (viewModel.SectionCoversIdSelected.Contains(sect.Id) && sect.SectionCoverId != viewModel.Photo.Id)
                {
                    sect.SectionCoverId = viewModel.Photo.Id;
                    _db.Section.Update(sect);
                }
                else if (!viewModel.SectionCoversIdSelected.Contains(sect.Id) && sect.SectionCoverId == viewModel.Photo.Id)
                {
                    sect.SectionCoverId = null;
                    _db.Section.Update(sect);
                }
            }

            //Update album covers
            foreach (Album alb in _db.Album)
            {
                if (viewModel.AlbumCoversIdSelected.Contains(alb.Id) && alb.CoverPhotoId != viewModel.Photo.Id)
                {
                    alb.CoverPhotoId = viewModel.Photo.Id;
                    _db.Album.Update(alb);
                }
                else if (!viewModel.AlbumCoversIdSelected.Contains(alb.Id) && alb.CoverPhotoId == viewModel.Photo.Id)
                {
                    alb.CoverPhotoId = null;
                    _db.Album.Update(alb);
                }
            }

            //Update review covers
            foreach (Review rev in _db.Review)
            {
                if (viewModel.ReviewsIdSelected.Contains(rev.Id) && rev.ReviewPhotoId != viewModel.Photo.Id)
                {
                    rev.ReviewPhotoId = viewModel.Photo.Id;
                    _db.Review.Update(rev);
                }
                else if (!viewModel.ReviewsIdSelected.Contains(rev.Id) && rev.ReviewPhotoId == viewModel.Photo.Id)
                {
                    rev.ReviewPhotoId = null;
                    _db.Review.Update(rev);
                }
            }

            //Update carousels
            foreach (Carousel car in _db.Carousel)
            {
                if (viewModel.CarouselsIdSelected.Contains(car.Id) && car.PhotoId != viewModel.Photo.Id)
                {
                    car.PhotoId = viewModel.Photo.Id;
                    _db.Carousel.Update(car);
                }
                else if (!viewModel.CarouselsIdSelected.Contains(car.Id) && car.PhotoId == viewModel.Photo.Id)
                {
                    car.PhotoId = null;
                    _db.Carousel.Update(car);
                }
            }

            //Update album contents
            foreach (AlbumPhoto ap in _db.AlbumPhoto.Where(x => x.PhotoId == viewModel.Photo.Id && !viewModel.AlbumsIdSelected.Contains(x.AlbumId)))
            {
                _db.AlbumPhoto.Remove(ap);
            }

            AlbumPhoto newAlbumPhoto;
            foreach (int albumId in viewModel.AlbumsIdSelected)
            {
                if (!(_db.AlbumPhoto.Any(x => x.AlbumId == albumId && x.PhotoId == viewModel.Photo.Id)))
                {
                    newAlbumPhoto = new AlbumPhoto()
                    {
                        PhotoId = viewModel.Photo.Id,
                        AlbumId = albumId
                    };
                    _db.AlbumPhoto.Add(newAlbumPhoto);
                }
            }

            _db.SaveChanges();
        }
        public void RemovePhoto(int photoId)
        {

        }

        public Photo CreatePhotoWithVersions(IFormFile formFile)
        {
            List<ResolutionConfig> resolutions = _db.ResolutionConfig.ToList();

            Photo newPhoto = new Photo()
            {
                IsHidden = false,
                NSFW = false
            };
            _db.Photo.Update(newPhoto);
            _db.SaveChanges();

            //Original photo
            string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, formFile);

            int originalX, originalY = 0;
            using (var image = Image.FromStream(formFile.OpenReadStream()))
            {
                originalX = image.Width;
                originalY = image.Height;
            }
            ;

            PhotoVersion originalVersion = new PhotoVersion()
            {
                Width = originalX,
                Height = originalY,
                Path = photoPath,
                PhotoId = newPhoto.Id,
                IsOriginal = true
            };
            _db.PhotoVersion.Update(originalVersion);

            //Resized versions
            foreach (ResolutionConfig res in resolutions)
            {
                CreatePhotoVersion(newPhoto, res.ShortSide);
            }

            _db.SaveChanges();

            return newPhoto;
        }

        public List<int> FindMissingVersions(Photo photo, float tolerance = 0.05f)
        {
            List<int> missingRes = new List<int>();
            List<ResolutionConfig> resolutions = _db.ResolutionConfig.ToList();

            foreach (ResolutionConfig res in resolutions)
            {
                if (!(photo.PhotoVersions.Any(x => Math.Min(x.Width, x.Height) > (res.ShortSide * 1f - tolerance) && Math.Min(x.Width, x.Height) < (res.ShortSide * 1f + tolerance))))
                {
                    missingRes.Add(res.ShortSide);
                }
            }

            return missingRes;
        }

        public PhotoVersion CreatePhotoVersion(Photo photo, int shortSide)
        {
            PhotoVersion original = photo.PhotoVersions.OrderByDescending(x => x.IsOriginal).ThenBy(x => Math.Max(x.Width, x.Height)).FirstOrDefault();
            if (original == null) return null; //should not happen

            //Resized versions
            double aspect = 1;
            int longSide = 0;
            int newWidth = 0;
            int newHeight = 0;

            if (shortSide > Math.Min(original.Width, original.Height)) return null; //no upsizing
            aspect = (double)original.Width / original.Height;
            longSide = (int)(((double)Math.Max(original.Width, original.Height) / Math.Min(original.Height, original.Width)) * shortSide);

            newWidth = (aspect > 1) ? longSide : shortSide;
            newHeight = (aspect > 1) ? shortSide : longSide;

            string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, original.Path, newWidth, newHeight);

            PhotoVersion newVersion = new PhotoVersion()
            {
                Width = newWidth,
                Height = newHeight,
                Path = photoPath,
                PhotoId = photo.Id
            };

            _db.PhotoVersion.Update(newVersion);
            return newVersion;
        }

        #endregion

        #region Album
        public void UpsertAlbum(UpsertAlbumViewModel viewModel)
        {
            Album newAlbum = viewModel.Album;

            if (viewModel.PreviewImage != null)
            {
                Photo newPhoto = CreatePhotoWithVersions(viewModel.PreviewImage);
                newAlbum.CoverPhotoId = newPhoto.Id;
            }

            _db.Album.Update(newAlbum);
            _db.SaveChanges();

            _db.AlbumSection.RemoveRange(_db.AlbumSection.Where(x => x.AlbumId == newAlbum.Id));
            _db.AlbumSection.AddRange(viewModel.SectionIdSelected.Select(x => new AlbumSection() { AlbumId = newAlbum.Id, SectionId = x }));

            _db.SaveChanges();
        }
        public void RemoveAlbum(int albumId)
        {

        }
        #endregion

        #region Section
        public void UpsertSection(UpsertSectionViewModel viewModel)
        {
            Section newSection = viewModel.Section;

            if (viewModel.PreviewImage != null)
            {
                Photo newPhoto = CreatePhotoWithVersions(viewModel.PreviewImage);
                newSection.SectionCoverId = newPhoto.Id;
            }
            _db.Section.Update(newSection);
            _db.SaveChanges();
        }
        public void RemoveSection(int sectionId)
        {

        }
        #endregion

        #region Review
        public void UpsertReview(UpsertReviewViewModel viewModel)
        {
            Review newReview = viewModel.Review;

            if (viewModel.PreviewImage != null)
            {
                Photo newPhoto = CreatePhotoWithVersions(viewModel.PreviewImage);
                newReview.ReviewPhotoId = newPhoto.Id;
            }

            _db.Review.Update(newReview);
            _db.SaveChanges();
        }
        public void RemoveReview(int reviewId)
        {

        }
        #endregion

        #region Carousel
        public void UpsertCarousel(UpsertCarouselViewModel viewModel)
        {
            Carousel existingCarousel;

            //Left
            if (viewModel.CarouselPhotoLeft != null)
            {
                existingCarousel = _db.Carousel.Where(x => x.Id == viewModel.CarouselLeft.Id).First();
                Photo newPhoto = CreatePhotoWithVersions(viewModel.CarouselPhotoLeft);
                existingCarousel.PhotoId = newPhoto.Id;
                _db.Carousel.Update(existingCarousel);
                _db.SaveChanges();
            }

            //Mid
            if (viewModel.CarouselPhotoMid != null)
            {
                existingCarousel = _db.Carousel.Where(x => x.Id == viewModel.CarouselMid.Id).First();
                Photo newPhoto = CreatePhotoWithVersions(viewModel.CarouselPhotoMid);
                existingCarousel.PhotoId = newPhoto.Id;
                _db.Carousel.Update(existingCarousel);
                _db.SaveChanges();
            }

            //Right
            if (viewModel.CarouselPhotoRight != null)
            {
                existingCarousel = _db.Carousel.Where(x => x.Id == viewModel.CarouselRight.Id).First();
                Photo newPhoto = CreatePhotoWithVersions(viewModel.CarouselPhotoRight);
                existingCarousel.PhotoId = newPhoto.Id;
                _db.Carousel.Update(existingCarousel);
                _db.SaveChanges();
            }
        }
        #endregion
    }
}