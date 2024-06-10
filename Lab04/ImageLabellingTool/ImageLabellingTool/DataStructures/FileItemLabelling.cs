using System.Windows.Media;

namespace ImageLabellingTool
{
    public class FileItemLabelling
    {
        public FileItem FileItem { get; set; }
        public Dictionary<LabelEntry, List<RectanglePoint>> LabellingDictionary { get; set; } = [];
        public FileItemLabelling(FileItem fileItem)
        {
            FileItem = fileItem;
        }
    }
}
