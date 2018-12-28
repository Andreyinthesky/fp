using System.Drawing;

namespace TagsCloud.Core.Settings
{
    public class ImageSettings : IImageSettings
    {
        public Size Size { get; set; }
        public int Width => Size.Width;
        public int Height => Size.Height;
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
    }
}