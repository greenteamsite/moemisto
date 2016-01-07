using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Moemisto.UI.Helpers
{
    public static class PictureResizer
    {
        public static Bitmap ResizeImage(Stream fullImageStream, int newWidth)
        {
            using (var image = Image.FromStream(fullImageStream))
            {
                float aspectRatio = (float) image.Size.Width/(float) image.Size.Height;
                int newHeight = Convert.ToInt32(newWidth/aspectRatio);
                var thumbnailBitmap = new Bitmap(newWidth, newHeight);

                using (var thumbnailGraph = Graphics.FromImage(thumbnailBitmap))
                {
                    thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    thumbnailGraph.DrawImage(image, imageRectangle);

                    return thumbnailBitmap;
                }
            }
        }
    }
}