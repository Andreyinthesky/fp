using System.Collections.Generic;
using System.Linq;
using TagsCloud.Core.WordConverters;
using TagsCloud.Core.WordFilters;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core
{
    public class TextPreprocessor : ITextPreprocessor
    {
        private readonly IWordConverter[] converters;
        private readonly IMapper<string, IEnumerable<string>> textSplitter;

        public TextPreprocessor(IWordConverter[] converters, IMapper<string, IEnumerable<string>> textSplitter)
        {
            this.converters = converters;
            this.textSplitter = textSplitter;
        }

        public Result<IEnumerable<string>> Process(string text)
        {
            return Result.Of(() => textSplitter.Map(text))
                .Then
                (
                    words => converters
                        .Aggregate(words, (convertedWords, converter) => convertedWords.Select(converter.ConvertWord))
                )
                .RefineError("Cannot preprocess text");
        }
    }
}