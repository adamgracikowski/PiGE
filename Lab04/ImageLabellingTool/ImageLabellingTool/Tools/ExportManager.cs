using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageLabellingTool
{
    public static class ExportManager
    {
        public static void TryCreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public static void ExportLabelling(Labelling labelling, Canvas canvas, string? directory = null)
        {
            directory ??= AppDomain.CurrentDomain.BaseDirectory;

            var name = labelling.Label.Name;
            var path = Path.Combine(directory, name);
            TryCreateDirectory(path);

            var counter = 0;
            foreach (var rectangle in labelling.Rectangles)
            {
                var bitmap = rectangle.Owner.FileItem.Image;
                var cropped = bitmap.CropToRectangle(rectangle, canvas);
                var filename = Path.Combine(path, $"{name}_{counter++}.png");
                cropped.SaveToFile(filename);
            }
        }
    }
}
