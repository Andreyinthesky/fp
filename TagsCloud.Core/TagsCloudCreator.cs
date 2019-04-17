using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloud.Core.Layouters;
using TagsCloud.Core.Providers;
using TagsCloud.Core.Settings;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core
{
    public class TagsCloudCreator : ITagsCloudCreator
    {
        private readonly ICloudLayouter layouter;
        private readonly IFrequencyWordsAnalyzer wordsAnalyzer;
        private readonly IProvider<IEnumerable<string>> sourceWordsProvider;

        public TagsCloudCreator(
            ICloudLayouter layouter,
            IFrequencyWordsAnalyzer wordsAnalyzer,
            IProvider<IEnumerable<string>> sourceWordsProvider
            )
        {
            this.layouter = layouter;
            this.wordsAnalyzer = wordsAnalyzer;
            this.sourceWordsProvider = sourceWordsProvider;
        }

        public Result<TagsCloud> CreateTagsCloud(string textFilePath, IFontSettings fontSettings)
        {
            return sourceWordsProvider.Get()
                .Then
                (
                    words => wordsAnalyzer
                        .Analyze(words)
                        .OrderByDescending(kvp => kvp.Value)
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                )
                .Then(frequencyByWord => GetTags(frequencyByWord, fontSettings))
                .Then(tags => new TagsCloud(tags, layouter.CloudWidth, layouter.CloudHeight, layouter.Center))
                .RefineError("Cannot create tags cloud");
        }

        private IEnumerable<Tag> GetTags(Dictionary<string, int> frequencyByWord, IFontSettings fontSettings)
        {
            var tags = new List<Tag>();

            if (!frequencyByWord.Values.Any())
                return tags;

            var minFrequency = frequencyByWord.Values.Min();
            var maxFrequency = frequencyByWord.Values.Max();

            foreach (var weightedWord in frequencyByWord)
            {
                var fontSize = GetFontSize(fontSettings, weightedWord.Value, minFrequency, maxFrequency);
                var font = new Font(fontSettings.FontFamily, fontSize, fontSettings.FontStyle, GraphicsUnit.Point);
                var frameSize = TextRenderer.MeasureText(weightedWord.Key, font);
                var frame = layouter.PutNextRectangle(frameSize);
                tags.Add(new Tag(weightedWord.Key, font, frame));
            }

            return tags;
        }

        private int GetFontSize(IFontSettings fontSettings, int currentFrequency, int minFrequency, int maxFrequency)
        {
            var minFontSize = fontSettings.MinFontSizeInPoints;
            var maxFontSize = fontSettings.MaxFontSizeInPoints;

            return currentFrequency > minFrequency
                ? Math.Max
                (
                    (int) Math.Ceiling((double) maxFontSize * (currentFrequency - minFrequency) / (maxFrequency - minFrequency)),
                    minFontSize
                )
                : minFontSize;
        }
    }
}