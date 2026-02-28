using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Portfolio.Models.Models;
using System.Drawing;
using System.Drawing.Imaging;

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

        public static string AddNewPhotoFile(IWebHostEnvironment env, IFormFile? image, int targetWidth = 0, int targetHeight = 0)
        {
			if (image == null) return "";
			if (string.IsNullOrWhiteSpace(image.FileName)) return "";

			string fileName = Guid.NewGuid() + Path.GetExtension(image?.FileName);

			string newPath = System.IO.Path.Join(PATH_PHOTOS + fileName);
			string fullPath = Path.Join(env.WebRootPath, newPath);

			using (var fileStream = new FileStream(fullPath, FileMode.Create))
			{
                if (targetWidth + targetHeight != 0)
                {
                    var tmpImage = System.Drawing.Image.FromStream(image.OpenReadStream());
                    var resized = new Bitmap(tmpImage, new Size(targetWidth, targetHeight));
                    resized.Save(fileStream, ImageFormat.Jpeg);
                }
                else
                {
                    image.CopyTo(fileStream);
                }
            }

			return newPath;
		}

		public static string PathOrPlaceholder(string path, bool vertical)
		{
			return string.IsNullOrWhiteSpace(path) ? GetPlaceholder(vertical) : path;
		}

        public static PhotoVersion ImageMatchSize(Photo photo, int shortSide)
        {
            if (photo?.PhotoVersions == null || photo?.PhotoVersions.Count()==0) return new PhotoVersion() { Path = GetPlaceholder(true), Width=1080, Height=1350 };
            if (shortSide == 0) return photo.PhotoVersions.OrderByDescending(x=>x.IsOriginal).ThenByDescending(x => Math.Min(x.Width, x.Height)).First();

            PhotoVersion ver = photo.PhotoVersions.OrderBy(x => Math.Abs(shortSide - Math.Min(x.Width, x.Height))).First();

            return ver != null ? ver : photo.PhotoVersions.First(x => x.IsOriginal);
        }
    }
}
