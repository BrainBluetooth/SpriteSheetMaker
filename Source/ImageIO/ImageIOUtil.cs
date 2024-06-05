using System.Drawing;
using System.IO;

namespace ImageIO
{
    public static class ImageIOUtil
    {
        public enum Error
        {
            Etc = -1,
            None = 0,
            FileNoutFound,
            NotImplemented,
        }

        public static Error TryLoadImage(string path, out Image image)
        {
            image = null;

            if (!File.Exists(path))
                return Error.FileNoutFound;

            string extension = Path.GetExtension(path);
            switch (extension)
            {
                case ".psd":
                    return Error.NotImplemented;
                default:
                    return TryLoadImage_GDIPlus(path, out image);
            }
        }
        private static Error TryLoadImage_GDIPlus(string path, out Image image)
        {
            image = null;
            try
            {
                Bitmap bmp = new Bitmap(path);
                image = bmp;
                return Error.None;
            }
            catch
            {
                return Error.Etc;
            }
        }

        public enum ImageFormat
        {
            None,
            GDIPlus,
        }
        public static Error TrySaveImage(string path, Image image, ImageFormat format)
        {
            if (format == ImageFormat.None)
            {
                string extension = Path.GetExtension(path);
                switch (extension)
                {
                    case ".psd":
                        return Error.NotImplemented;
                    default:
                        format = ImageFormat.GDIPlus;
                        break;
                }
            }
            switch (format)
            {
                case ImageFormat.GDIPlus:
                    return TrySaveImage_GDIPlus(path, image);
                default:
                    return Error.Etc;
            }
        }
        private static Error TrySaveImage_GDIPlus(string path, Image image)
        {
            try
            {
                image.Save(path);
                return Error.None;
            }
            catch
            {
                return Error.Etc;
            }
        }
    }
}