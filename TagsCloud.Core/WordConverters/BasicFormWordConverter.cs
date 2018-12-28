using System;
using System.Linq;
using NHunspell;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core.WordConverters
{
    public class BasicFormWordConverter : IWordConverter, IDisposable
    {
        private readonly Hunspell hunspell;

        public BasicFormWordConverter()
        {
            this.hunspell = Result.Of(() => new Hunspell("RU_Dict/ru_RU.aff", "RU_Dict/ru_RU.dic"))
                .RefineError("Hunsplell library has failed")
                .GetValueOrThrow();
        }

        public string ConvertWord(string word)
        {
            return hunspell.Stem(word).Any() ? hunspell.Stem(word).First() : word;
        }

        public void Dispose()
        {
            hunspell?.Dispose();
        }
    }
}