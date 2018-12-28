using System.Collections.Generic;
using TagsCloud.Core.FileReaders;
using TagsCloud.Core.Settings;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core.Providers
{
    public class StopWordsProvider : IProvider<IEnumerable<string>>
    {
        private readonly CloudSettings cloudSettings;
        private readonly ITextFileReader fileReader;
        private readonly ITextPreprocessor textPreprocessor;

        public StopWordsProvider(CloudSettings cloudSettings, ITextFileReader fileReader, ITextPreprocessor textPreprocessor)
        {
            this.cloudSettings = cloudSettings;
            this.fileReader = fileReader;
            this.textPreprocessor = textPreprocessor;
        }

        public Result<IEnumerable<string>> Get()
        {
            return fileReader
                .ReadText(cloudSettings.InputStopWordsFilePath)
                .Then(text => textPreprocessor.Process(text))
                .RefineError("Cannot provide stop words");
        }
    }
}