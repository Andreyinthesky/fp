using TagsCloud.Core.Settings;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core
{
    public interface ITagsCloudCreator
    {
        Result<TagsCloud> CreateTagsCloud(string textFilePath, FontSettings fontSettings);
    }
}