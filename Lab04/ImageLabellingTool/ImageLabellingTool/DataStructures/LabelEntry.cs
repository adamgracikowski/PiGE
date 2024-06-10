﻿using System.Windows.Media;

namespace AILabelTool
{
    public class LabelEntry
    {
        public string Name { get; set; }
        public SolidColorBrush Color { get; set; }

        public LabelEntry(string name, SolidColorBrush color)
        {
            Name = name;
            Color = color;
        }
    }
}
