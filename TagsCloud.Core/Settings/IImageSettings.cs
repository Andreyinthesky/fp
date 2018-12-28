using System.Drawing;

namespace TagsCloud.Core.Settings
{
    public interface IImageSettings
    {
        int Width { get; }
        int Height { get; }
        Size Size { get; set; }
        Color ForegroundColor { get; set; }
        Color BackgroundColor { get; set; }
    }
}