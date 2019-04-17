namespace TagsCloud.Core.Settings
{
    public interface ICloudSettings
    {
        string InputTextFilePath { get; set; }
        string InputStopWordsFilePath { get; set; }
        int WordsCount { get; set; }
        IImageSettings ImageSettings{ get; set; }
        IFontSettings FontSettings{ get; set; }
    }
}