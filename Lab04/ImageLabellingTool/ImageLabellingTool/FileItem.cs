using System.IO;
using System.Windows.Media.Imaging;

namespace ImageLabellingTool
{
    public class FileItem
    {
        public static readonly BitmapImage FolderImage
            = new(new Uri("Images/folder-icon.png", UriKind.Relative));
        public static readonly BitmapImage NoPreviewImage
            = new(new Uri("Images/no-preview.png", UriKind.Relative));

        public FileItem(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            Image = ChooseImage(path);
        }

        public string Path { get; set; }
        public string Name { get; set; }
        public BitmapImage Image { get; set; }

        private static BitmapImage? CreateImage(string path)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static BitmapImage ChooseImage(string path)
        {
            if (Directory.Exists(path))
                return FolderImage;
            else if (File.Exists(path))
                return CreateImage(path);
            return NoPreviewImage;
        }
    }
}
