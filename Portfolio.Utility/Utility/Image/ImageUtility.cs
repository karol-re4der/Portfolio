using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Portfolio.Utility.Utility.Image
{
    public static class ImageUtility
    {
        public const string PATH_PHOTOS = "/res/photo/";
		public const string PATH_PLACEHOLDERS = "/res/photo/placeholder/";
		public const int ALBUM_COVER_WIDTH = 400;
		public const int ALBUM_COVER_HEIGHT = 500;
        public const int SECTION_COVER_WIDTH = 400;
        public const int SECTION_COVER_HEIGHT = 500;
        public const int REVIEW_COVER_WIDTH = 140;
        public const int REVIEW_COVER_HEIGHT = 140;

        public static string GetPlaceholder(bool vertical)
        {
            return PATH_PLACEHOLDERS + "placeholder_" + (vertical ? "vertical" : "horizontal") + ".jpg";
        }

        public static string AddNewPhotoFile(IWebHostEnvironment env, IFormFile? image)
        {
			if (image == null) return "";
			if (string.IsNullOrWhiteSpace(image.FileName)) return "";

			string fileName = Guid.NewGuid() + Path.GetExtension(image?.FileName);

			string newPath = System.IO.Path.Join(PATH_PHOTOS + fileName);
			string fullPath = Path.Join(env.WebRootPath, newPath);

			using (var fileStream = new FileStream(fullPath, FileMode.Create))
			{
				image.CopyTo(fileStream);
			}

			return newPath;
		}

		public static string PathOrPlaceholder(string path, bool vertical)
		{
			return string.IsNullOrWhiteSpace(path) ? GetPlaceholder(vertical) : path;
		}
    }
}
