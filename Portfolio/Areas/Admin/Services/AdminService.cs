using Microsoft.AspNetCore.Identity;
using Portfolio.Areas.Admin.Controllers;
using Portfolio.Areas.Admin.Services.Interfaces;
using Portfolio.DataAccess.Data;
using Portfolio.Models.Models;
using Portfolio.Models.Models.ViewModels;
using Portfolio.Utility.Utility.Image;
using System.Drawing;

namespace Portfolio.Areas.Admin.Services
{
    public class AdminService: IAdminService
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
        public void UpsertPhotos(UpsertPhotosViewModel viewModel){
            Album album = viewModel.Album;
            List<ResolutionConfig> resolutions = _db.ResolutionConfig.ToList();

            if (viewModel.PhotosUploaded != null && viewModel.PhotosUploaded.Count() > 0)
            {


                foreach (FormFile file in viewModel.PhotosUploaded)
                {
                    Photo newPhoto = new Photo()
                    {
                        IsHidden = false,
                        NSFW = false
                    };
                    _db.Photo.Update(newPhoto);
                    _db.SaveChanges();
                    _db.AlbumPhoto.Add(new AlbumPhoto() { AlbumId = album.Id, PhotoId = newPhoto.Id });
                    _db.SaveChanges();

                    //Original photo
                    string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, file);

                    int originalX, originalY = 0;
                    using (var image = Image.FromStream(file.OpenReadStream()))
                    {
                        originalX = image.Width;
                        originalY = image.Height;
                    }
                    ;

                    PhotoVersion newVersion = new PhotoVersion()
                    {
                        Width = originalX,
                        Height = originalY,
                        Path = photoPath,
                        PhotoId = newPhoto.Id,
                        IsOriginal = true
                    };
                    _db.PhotoVersion.Update(newVersion);
                    _db.SaveChanges();

                    //Resized versions
                    double aspect = 1;
                    int longSide = 0;
                    int newWidth = 0;
                    int newHeight = 0;

                    foreach (ResolutionConfig res in resolutions)
                    {
                        if (res.ShortSide > Math.Min(originalX, originalY)) continue; //no upsizing
                        aspect = (double)originalX / originalY;
                        longSide = (int)(((double)Math.Max(originalX, originalY) / Math.Min(originalX, originalY)) * res.ShortSide);

                        newWidth = (aspect > 1) ? longSide : res.ShortSide;
                        newHeight = (aspect > 1) ? res.ShortSide : longSide;

                        photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, file, newWidth, newHeight);

                        newVersion = new PhotoVersion()
                        {
                            Width = newWidth,
                            Height = newHeight,
                            Path = photoPath,
                            PhotoId = newPhoto.Id
                        };

                        _db.PhotoVersion.Update(newVersion);
                        _db.SaveChanges();
                    }
                }
            }
        }

        #endregion

        #region Photo
        public void UpsertPhoto(UpsertPhotoViewModel viewModel){

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
        #endregion

        #region Album
        public void UpsertAlbum(UpsertAlbumViewModel viewModel)
        {
            Album newAlbum = viewModel.Album;
            List<ResolutionConfig> resolutions = _db.ResolutionConfig.ToList();

            if (viewModel.PreviewImage != null)
            {
                Photo newPhoto = new Photo()
                {
                    IsHidden = false,
                    NSFW = false
                };
                _db.Photo.Update(newPhoto);
                _db.SaveChanges();
                newAlbum.CoverPhotoId = newPhoto.Id;

                //Original photo
                string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.PreviewImage);

                int originalX, originalY = 0;
                using (var image = Image.FromStream(viewModel.PreviewImage.OpenReadStream()))
                {
                    originalX = image.Width;
                    originalY = image.Height;
                }
                ;

                PhotoVersion newVersion = new PhotoVersion()
                {
                    Width = originalX,
                    Height = originalY,
                    Path = photoPath,
                    PhotoId = newPhoto.Id,
                    IsOriginal = true
                };
                _db.PhotoVersion.Update(newVersion);
                _db.SaveChanges();

                //Resized versions
                double aspect = 1;
                int longSide = 0;
                int newWidth = 0;
                int newHeight = 0;

                foreach (ResolutionConfig res in resolutions)
                {
                    if (res.ShortSide > Math.Min(originalX, originalY)) continue; //no upsizing
                    aspect = (double)originalX / originalY;
                    longSide = (int)(((double)Math.Max(originalX, originalY) / Math.Min(originalX, originalY)) * res.ShortSide);

                    newWidth = (aspect > 1) ? longSide : res.ShortSide;
                    newHeight = (aspect > 1) ? res.ShortSide : longSide;

                    photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.PreviewImage, newWidth, newHeight);

                    newVersion = new PhotoVersion()
                    {
                        Width = newWidth,
                        Height = newHeight,
                        Path = photoPath,
                        PhotoId = newPhoto.Id
                    };

                    _db.PhotoVersion.Update(newVersion);
                    _db.SaveChanges();
                }
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
            List<ResolutionConfig> resolutions = _db.ResolutionConfig.ToList();

            if (viewModel.PreviewImage != null)
            {
                Photo newPhoto = new Photo()
                {
                    IsHidden = false,
                    NSFW = false
                };
                _db.Photo.Update(newPhoto);
                _db.SaveChanges();
                newSection.SectionCoverId = newPhoto.Id;

                //Original photo
                string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.PreviewImage);

                int originalX, originalY = 0;
                using (var image = Image.FromStream(viewModel.PreviewImage.OpenReadStream()))
                {
                    originalX = image.Width;
                    originalY = image.Height;
                }
                ;

                PhotoVersion newVersion = new PhotoVersion()
                {
                    Width = originalX,
                    Height = originalY,
                    Path = photoPath,
                    PhotoId = newPhoto.Id,
                    IsOriginal = true
                };
                _db.PhotoVersion.Update(newVersion);
                _db.SaveChanges();

                //Resized versions
                double aspect = 1;
                int longSide = 0;
                int newWidth = 0;
                int newHeight = 0;

                foreach (ResolutionConfig res in resolutions)
                {
                    if (res.ShortSide > Math.Min(originalX, originalY)) continue; //no upsizing
                    aspect = (double)originalX / originalY;
                    longSide = (int)(((double)Math.Max(originalX, originalY) / Math.Min(originalX, originalY)) * res.ShortSide);

                    newWidth = (aspect > 1) ? longSide : res.ShortSide;
                    newHeight = (aspect > 1) ? res.ShortSide : longSide;

                    photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.PreviewImage, newWidth, newHeight);

                    newVersion = new PhotoVersion()
                    {
                        Width = newWidth,
                        Height = newHeight,
                        Path = photoPath,
                        PhotoId = newPhoto.Id
                    };

                    _db.PhotoVersion.Update(newVersion);
                    _db.SaveChanges();
                }
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
            List<ResolutionConfig> resolutions = _db.ResolutionConfig.ToList();

            if (viewModel.PreviewImage != null)
            {

                Photo newPhoto = new Photo()
                {
                    IsHidden = false,
                    NSFW = false
                };
                _db.Photo.Update(newPhoto);
                _db.SaveChanges();
                newReview.ReviewPhotoId = newPhoto.Id;

                //Original photo
                string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.PreviewImage);

                int originalX, originalY = 0;
                using (var image = Image.FromStream(viewModel.PreviewImage.OpenReadStream()))
                {
                    originalX = image.Width;
                    originalY = image.Height;
                }
                ;

                PhotoVersion newVersion = new PhotoVersion()
                {
                    Width = originalX,
                    Height = originalY,
                    Path = photoPath,
                    PhotoId = newPhoto.Id,
                    IsOriginal = true
                };
                _db.PhotoVersion.Update(newVersion);
                _db.SaveChanges();

                //Resized versions
                double aspect = 1;
                int longSide = 0;
                int newWidth = 0;
                int newHeight = 0;

                foreach (ResolutionConfig res in resolutions)
                {
                    if (res.ShortSide > Math.Min(originalX, originalY)) continue; //no upsizing
                    aspect = (double)originalX / originalY;
                    longSide = (int)(((double)Math.Max(originalX, originalY) / Math.Min(originalX, originalY)) * res.ShortSide);

                    newWidth = (aspect > 1) ? longSide : res.ShortSide;
                    newHeight = (aspect > 1) ? res.ShortSide : longSide;

                    photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.PreviewImage, newWidth, newHeight);

                    newVersion = new PhotoVersion()
                    {
                        Width = newWidth,
                        Height = newHeight,
                        Path = photoPath,
                        PhotoId = newPhoto.Id
                    };

                    _db.PhotoVersion.Update(newVersion);
                    _db.SaveChanges();
                }
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
            List<ResolutionConfig> resolutions = _db.ResolutionConfig.ToList();
            Carousel existingCarousel;

            //Left
            if (viewModel.CarouselPhotoLeft != null)
            {
                existingCarousel = _db.Carousel.Where(x => x.Id == viewModel.CarouselLeft.Id).First();

                Photo newPhoto = new Photo()
                {
                    IsHidden = false,
                    NSFW = false
                };
                _db.Photo.Update(newPhoto);
                _db.SaveChanges();
                existingCarousel.PhotoId = newPhoto.Id;

                //Original photo
                string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.CarouselPhotoLeft);

                int originalX, originalY = 0;
                using (var image = Image.FromStream(viewModel.CarouselPhotoLeft.OpenReadStream()))
                {
                    originalX = image.Width;
                    originalY = image.Height;
                }
                ;

                PhotoVersion newVersion = new PhotoVersion()
                {
                    Width = originalX,
                    Height = originalY,
                    Path = photoPath,
                    PhotoId = newPhoto.Id,
                    IsOriginal = true
                };
                _db.PhotoVersion.Update(newVersion);
                _db.SaveChanges();

                //Resized versions
                double aspect = 1;
                int longSide = 0;
                int newWidth = 0;
                int newHeight = 0;

                foreach (ResolutionConfig res in resolutions)
                {
                    if (res.ShortSide > Math.Min(originalX, originalY)) continue; //no upsizing
                    aspect = (double)originalX / originalY;
                    longSide = (int)(((double)Math.Max(originalX, originalY) / Math.Min(originalX, originalY)) * res.ShortSide);

                    newWidth = (aspect > 1) ? longSide : res.ShortSide;
                    newHeight = (aspect > 1) ? res.ShortSide : longSide;

                    photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.CarouselPhotoLeft, newWidth, newHeight);

                    newVersion = new PhotoVersion()
                    {
                        Width = newWidth,
                        Height = newHeight,
                        Path = photoPath,
                        PhotoId = newPhoto.Id
                    };

                    _db.PhotoVersion.Update(newVersion);
                    _db.SaveChanges();
                }
                _db.Carousel.Update(existingCarousel);
                _db.SaveChanges();
            }

            //Mid
            if (viewModel.CarouselPhotoMid != null)
            {
                existingCarousel = _db.Carousel.Where(x => x.Id == viewModel.CarouselMid.Id).First();

                Photo newPhoto = new Photo()
                {
                    IsHidden = false,
                    NSFW = false
                };
                _db.Photo.Update(newPhoto);
                _db.SaveChanges();
                existingCarousel.PhotoId = newPhoto.Id;

                //Original photo
                string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.CarouselPhotoMid);

                int originalX, originalY = 0;
                using (var image = Image.FromStream(viewModel.CarouselPhotoMid.OpenReadStream()))
                {
                    originalX = image.Width;
                    originalY = image.Height;
                }
                ;

                PhotoVersion newVersion = new PhotoVersion()
                {
                    Width = originalX,
                    Height = originalY,
                    Path = photoPath,
                    PhotoId = newPhoto.Id,
                    IsOriginal = true
                };
                _db.PhotoVersion.Update(newVersion);
                _db.SaveChanges();

                //Resized versions
                double aspect = 1;
                int longSide = 0;
                int newWidth = 0;
                int newHeight = 0;

                foreach (ResolutionConfig res in resolutions)
                {
                    if (res.ShortSide > Math.Min(originalX, originalY)) continue; //no upsizing
                    aspect = (double)originalX / originalY;
                    longSide = (int)(((double)Math.Max(originalX, originalY) / Math.Min(originalX, originalY)) * res.ShortSide);

                    newWidth = (aspect > 1) ? longSide : res.ShortSide;
                    newHeight = (aspect > 1) ? res.ShortSide : longSide;

                    photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.CarouselPhotoMid, newWidth, newHeight);

                    newVersion = new PhotoVersion()
                    {
                        Width = newWidth,
                        Height = newHeight,
                        Path = photoPath,
                        PhotoId = newPhoto.Id
                    };

                    _db.PhotoVersion.Update(newVersion);
                    _db.SaveChanges();
                }
                _db.Carousel.Update(existingCarousel);
                _db.SaveChanges();
            }

            //Right
            if (viewModel.CarouselPhotoRight != null)
            {
                existingCarousel = _db.Carousel.Where(x => x.Id == viewModel.CarouselRight.Id).First();

                Photo newPhoto = new Photo()
                {
                    IsHidden = false,
                    NSFW = false
                };
                _db.Photo.Update(newPhoto);
                _db.SaveChanges();
                existingCarousel.PhotoId = newPhoto.Id;

                //Original photo
                string photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.CarouselPhotoRight);

                int originalX, originalY = 0;
                using (var image = Image.FromStream(viewModel.CarouselPhotoRight.OpenReadStream()))
                {
                    originalX = image.Width;
                    originalY = image.Height;
                }
                ;

                PhotoVersion newVersion = new PhotoVersion()
                {
                    Width = originalX,
                    Height = originalY,
                    Path = photoPath,
                    PhotoId = newPhoto.Id,
                    IsOriginal = true
                };
                _db.PhotoVersion.Update(newVersion);
                _db.SaveChanges();

                //Resized versions
                double aspect = 1;
                int longSide = 0;
                int newWidth = 0;
                int newHeight = 0;

                foreach (ResolutionConfig res in resolutions)
                {
                    if (res.ShortSide > Math.Min(originalX, originalY)) continue; //no upsizing
                    aspect = (double)originalX / originalY;
                    longSide = (int)(((double)Math.Max(originalX, originalY) / Math.Min(originalX, originalY)) * res.ShortSide);

                    newWidth = (aspect > 1) ? longSide : res.ShortSide;
                    newHeight = (aspect > 1) ? res.ShortSide : longSide;

                    photoPath = ImageUtility.AddNewPhotoFile(_webHostEnvironment, viewModel.CarouselPhotoRight, newWidth, newHeight);

                    newVersion = new PhotoVersion()
                    {
                        Width = newWidth,
                        Height = newHeight,
                        Path = photoPath,
                        PhotoId = newPhoto.Id
                    };

                    _db.PhotoVersion.Update(newVersion);
                    _db.SaveChanges();
                }
                _db.Carousel.Update(existingCarousel);
                _db.SaveChanges();
            }
        }
        #endregion
    }
}
