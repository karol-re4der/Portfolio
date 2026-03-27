using Portfolio.Models.Models;
using Portfolio.Models.Models.ViewModels;

namespace Portfolio.Areas.Admin.Services.Interfaces
{
    public interface IAdminService
    {

        #region Photos
        public void UpsertPhotos(UpsertPhotosViewModel viewModel);

        #endregion

        #region Photo
        public void UpsertPhoto(UpsertPhotoViewModel viewModel);
        public void RemovePhoto(int photoId);
        public List<int> FindMissingVersions(Photo photo, float tolerance = 0.05f);
        public Photo CreatePhotoWithVersions(IFormFile formFile);
        public PhotoVersion CreatePhotoVersion(Photo photo, int shortSide);

        #endregion

        #region Album
        public void UpsertAlbum(UpsertAlbumViewModel viewModel);
        public void RemoveAlbum(int albumId);

        #endregion

        #region Section
        public void UpsertSection(UpsertSectionViewModel viewModel);
        public void RemoveSection(int sectionId);

        #endregion

        #region Review
        public void UpsertReview(UpsertReviewViewModel viewModel);
        public void RemoveReview(int reviewId);

        #endregion

        #region Carousel
        public void UpsertCarousel(UpsertCarouselViewModel viewModel);
        #endregion
    }
}
