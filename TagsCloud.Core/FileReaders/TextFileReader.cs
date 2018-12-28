using System.IO;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core.FileReaders
{
    public class TextFileReader : ITextFileReader
    {
        public Result<string> ReadText(string filePath)
        {
            if (File.Exists(filePath))
            {
                return Result
                    .Of(() => File.ReadAllText(filePath))
                    .ReplaceError(e => "File couldn't be read");
            }

            return Result.Fail<string>($"File {filePath} doesn't exists");
        }
    }
}