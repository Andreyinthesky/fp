namespace TagsCloud.Core.Settings
{
    public class CloudSettings : ICloudSettings
    {
        public string InputTextFilePath { get; set; } = "Samples/sample_words.txt";
        public string InputStopWordsFilePath { get; set; } = "Samples/sample_stopwords.txt";
        public int WordsCount { get; set; } = 50;
        public IImageSettings ImageSettings { get; set; }
        public IFontSettings FontSettings  { get; set; }

        public CloudSettings(IImageSettings imageSettings, IFontSettings fontSettings)
        {
            ImageSettings = imageSettings;
            FontSettings = fontSettings;
        }
    }
}