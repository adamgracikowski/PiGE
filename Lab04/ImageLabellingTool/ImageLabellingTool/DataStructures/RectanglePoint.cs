using System.Windows;
using System.Windows.Shapes;

namespace ImageLabellingTool
{
    public class RectanglePoint
    {
        public FileItemLabelling Owner { get; set; }
        public Rectangle Rectangle { get; set; }
        public Point Point { get; set; }

        public RectanglePoint(Rectangle rectangle, Point point, FileItemLabelling owner)
        {
            Rectangle = rectangle;
            Point = point;
            Owner = owner;
        }
    }
}
