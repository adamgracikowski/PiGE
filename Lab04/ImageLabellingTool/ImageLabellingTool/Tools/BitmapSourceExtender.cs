using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ImageLabellingTool
{
    public static class BitmapSourceExtender
    {
        public static void SaveToFile(this BitmapSource bitmapSource, string path)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            using var stream = new FileStream(path, FileMode.Create);
            encoder.Save(stream);
        }
        public static CroppedBitmap CropToRectangle(this BitmapSource bitmap, RectanglePoint rectangle, Canvas canvas)
        {

            var scaleX = bitmap.PixelWidth / canvas.ActualWidth;
            var scaleY = bitmap.PixelHeight / canvas.ActualHeight;

            var x = rectangle.Point.X * scaleX;
            var y = rectangle.Point.Y * scaleY;
            var width = rectangle.Rectangle.Width * scaleX;
            var height = rectangle.Rectangle.Height * scaleY;

            return new CroppedBitmap(bitmap,
                new Int32Rect((int)x, (int)y, (int)width, (int)height));
        }
    }
}
