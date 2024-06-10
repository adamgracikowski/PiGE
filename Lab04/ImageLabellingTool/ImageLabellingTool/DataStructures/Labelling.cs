using System.Windows;
using System.Windows.Media;

namespace AILabelTool
{
    public class Labelling
    {
        public LabelEntry Label { get; set; }
        public List<RectanglePoint> Rectangles { get; set; } = [];
        public Labelling(LabelEntry label)
        {
            Label = label;
        }
        public Labelling(string name, SolidColorBrush color)
        {
            Label = new LabelEntry(name, color);
        }

    }
}
