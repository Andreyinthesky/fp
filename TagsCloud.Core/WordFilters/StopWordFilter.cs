using System.Collections.Generic;
using System.Linq;
using TagsCloud.Core.Providers;

namespace TagsCloud.Core.WordFilters
{
    public class StopWordFilter : IWordFilter
    {
        private readonly IProvider<IEnumerable<string>> stopWordsProvider;
        private HashSet<string> stopWords;

        public StopWordFilter(IProvider<IEnumerable<string>> stopWordsProvider)
        {
            this.stopWordsProvider = stopWordsProvider;
        }

        public bool Filter(string word)
        {
            if (stopWords == null)
            {
                stopWords = GetStopWords();
            }

            return !stopWords.Contains(word);
        }

        private HashSet<string> GetStopWords()
        {
            return stopWordsProvider
                .Get()
                .GetValueOrThrow()
                .ToHashSet();
        }
    }
}