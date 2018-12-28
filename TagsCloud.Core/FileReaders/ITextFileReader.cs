using TagsCloud.ErrorHandling;

namespace TagsCloud.Core.FileReaders
{
    public interface ITextFileReader
    {
        Result<string> ReadText(string filePath);
    }
}