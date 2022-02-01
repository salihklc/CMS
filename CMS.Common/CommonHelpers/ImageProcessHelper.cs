using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;


namespace CMS.Common.Helpers
{
    public static class ImageProcessHelper
    {
        public static string CreateBase64ThumbFromImage(string FilePath, int quality, int size, int width = 0, int height = 0)
        {

            string base64ThumbImage;

            using (Image image = Image.Load(FilePath))
            {
                if (width == 0 || height == 0)
                {
                    height = 60;
                    width = 60;
                }

                if (image.Width > image.Height)
                {
                    width = size;
                    height = Convert.ToInt32(image.Height * size / (double)image.Width);
                }
                else
                {
                    width = Convert.ToInt32(image.Width * size / (double)image.Height);
                    height = size;
                }

                using (var output = new MemoryStream())
                {
                    image.Mutate(m => m.Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max
                    })
                    );

                    image.SaveAsBmp(output);

                    base64ThumbImage = Convert.ToBase64String(output.ToArray());
                }
            }

            return base64ThumbImage;
        }
    }
}