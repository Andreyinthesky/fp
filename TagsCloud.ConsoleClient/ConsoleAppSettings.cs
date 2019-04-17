using CommandLine;

namespace TagsCloud.ConsoleClient
{
    public class ConsoleAppSettings
    {
        [Option('f', "text_file", HelpText = "input text file path", Default = "Samples/sample_words.txt")]
        public string TextFilePath { get; set; }

        [Option('s', "text_file", HelpText = "input stop words file path",
            Default = "Samples/sample_stopwords.txt")]
        public string StopWordsFilePath { get; set; }

        [Option("fg", HelpText = "font color", Default = "Black")]
        public string ForegroundColor { get; set; }

        [Option("bg", HelpText = "background color", Default = "White")]
        public string BackgroundColor { get; set; }

        [Option('t', "typeface", HelpText = "font typeface for tags", Default = "Segoe UI")]
        public string TypeFace { get; set; }

        [Option('w', "width", HelpText = "width of output image", Default = 1000)]
        public int ImageWidth { get; set; }

        [Option('h', "height", HelpText = "height of output image", Default = 1000)]
        public int ImageHeight { get; set; }

        [Option('c', "count", HelpText = "words count in the cloud", Default = 50)]
        public int WordsCount { get; set; }
    }
}