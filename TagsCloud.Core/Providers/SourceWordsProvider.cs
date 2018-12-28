using System.Collections.Generic;
using TagsCloud.Core.FileReaders;
using TagsCloud.Core.Settings;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core.Providers
{
    public class SourceWordsProvider : IProvider<IEnumerable<string>>
    {
        private readonly ICloudSettings cloudSettings;
        private readonly ITextFileReader fileReader;
        private readonly ITextPreprocessor textPreprocessor;

        public SourceWordsProvider(ICloudSettings cloudSettings, ITextFileReader fileReader, ITextPreprocessor textPreprocessor)
        {
            this.cloudSettings = cloudSettings;
            this.fileReader = fileReader;
            this.textPreprocessor = textPreprocessor;
        }

        public Result<IEnumerable<string>> Get()
        {
            return fileReader
                .ReadText(cloudSettings.InputTextFilePath)
                .Then(text => textPreprocessor.Process(text))
                .RefineError("Cannot provide source words");
        }
    }
}