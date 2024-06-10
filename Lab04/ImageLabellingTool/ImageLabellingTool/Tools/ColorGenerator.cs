using System.Windows.Media;


namespace ImageLabellingTool
{
    public static class ColorGenerator
    {
        public static byte NextByte()
        {
            return (byte)Random.Shared.Next(0, MaxByte);
        }
        public static Color NextColor()
        {
            var r = NextByte();
            var g = NextByte();
            var b = NextByte();
            return Color.FromRgb(r, g, b);
        }
        private static readonly int MaxByte = 256;
    }
}
