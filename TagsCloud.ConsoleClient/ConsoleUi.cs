using CommandLine;
using System;
using System.Drawing;
using System.IO;
using TagsCloud.Core;
using TagsCloud.Core.Settings;
using TagsCloud.ErrorHandling;

namespace TagsCloud.ConsoleClient
{
    public class ConsoleUi : IUserInterface
    {
        private readonly ITagsCloudVisualizer visualizer;
        private readonly ICloudSettings cloudSettings;

        public ConsoleUi(ITagsCloudVisualizer visualizer, ICloudSettings cloudSettings)
        {
            this.visualizer = visualizer;
            this.cloudSettings = cloudSettings;
        }

        public void Run(string[] args)
        {
            var outputFilePath = "output.png";

            Parser.Default.ParseArguments<ConsoleAppSettings>(args)
                .WithNotParsed(errors => Terminate(string.Join(" ", errors)))
                .WithParsed(appSettings =>
                {
                    UpdateCloudSettings(appSettings);
                    visualizer.GetCloudImage()
                        .Then(image => image.Save(outputFilePath))
                        .OnFail(e => Terminate(e));
                });

            Console.WriteLine($"Image save to {Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), outputFilePath)}");
        }

        private void Terminate(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }

        private void UpdateCloudSettings(ConsoleAppSettings appSettings)
        {
            Result.Of(() => SetTextFilePath(cloudSettings, appSettings.TextFilePath))
                .Then(settings => SetStopWordsFilePath(settings, appSettings.StopWordsFilePath))
                .Then(settings => SetImageSize(settings, new Size(appSettings.ImageWidth, appSettings.ImageHeight)))   
                .Then(settings => SetFontTypeFace(settings, appSettings.TypeFace))
                .Then(settings => SetWordsCount(settings, appSettings.WordsCount))
                .Then(settings => SetForegroundColor(settings, appSettings.ForegroundColor))
                .Then(settings => SetBackgroundColor(settings, appSettings.BackgroundColor))
                .RefineError("Failed to parse arguments")
                .OnFail(e => Terminate(e));
        }

        private ICloudSettings SetTextFilePath(ICloudSettings settings, string textFilePath)
        {
            settings.InputTextFilePath = textFilePath;
            return settings;
        }

        private ICloudSettings SetStopWordsFilePath(ICloudSettings settings, string stopWordsFilePath)
        {
            settings.InputStopWordsFilePath = stopWordsFilePath;
            return settings;
        }

        private Result<ICloudSettings> SetForegroundColor(ICloudSettings settings, string foregroundColorName)
        {
            settings.ImageSettings.ForegroundColor = Color.FromName(foregroundColorName);
            return Validate(settings, x => x.ImageSettings.ForegroundColor.IsKnownColor,
                $"Font color {foregroundColorName} is unknown.");
        }

        private Result<ICloudSettings> SetBackgroundColor(ICloudSettings settings, string backgroundColorName)
        {
            settings.ImageSettings.BackgroundColor = Color.FromName(backgroundColorName);
            return Validate(settings, x => x.ImageSettings.BackgroundColor.IsKnownColor,
                $"Background color {backgroundColorName} is unknown.");
        }

        private Result<ICloudSettings> SetImageSize(ICloudSettings settings, Size size)
        {
            settings.ImageSettings.Size = size;
            return Validate(settings, x => IsImageSizeCorrect(x.ImageSettings.Size), "Size is incorrect");
        }

        private Result<ICloudSettings> SetFontTypeFace(ICloudSettings settings, string typeFaceName)
        {
            return Result.Of(() => new FontFamily(typeFaceName), $"Type face name \"{typeFaceName}\" is not available")
                .Then(x => ChangeFont(settings, x));
        }

        private Result<ICloudSettings> SetWordsCount(ICloudSettings settings, int wordsCount)
        {
            settings.WordsCount = wordsCount;
            return Validate(settings, x => IsWordsCountCorrect(settings.WordsCount),
                $"Words count should be in range 1 to 1000, but was {wordsCount}");
        }

        private ICloudSettings ChangeFont(ICloudSettings settings, FontFamily fontFamily)
        {
            settings.FontSettings.FontFamily = fontFamily;
            return settings;
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> validate, string errorMessage)
        {
            return validate(obj) ? Result.Ok(obj) : Result.Fail<T>(errorMessage);
        }

        private bool IsImageSizeCorrect(Size size)
            => size.Height > 0 && size.Width > 0;

        private bool IsWordsCountCorrect(int count)
            => count > 0 && count <= 1000;
    }
}