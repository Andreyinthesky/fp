using System.Drawing;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core
{
    public interface ITagsCloudVisualizer
    {
        Result<Bitmap> GetCloudImage();
    }
}