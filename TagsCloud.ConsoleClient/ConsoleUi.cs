using System;
using System.IO;
using TagsCloud.Core;
using TagsCloud.ErrorHandling;

namespace TagsCloud.ConsoleClient
{
    public class ConsoleUi : IUserInterface
    {
        private readonly ITagsCloudVisualizer visualizer;

        public ConsoleUi(ITagsCloudVisualizer visualizer)
        {
            this.visualizer = visualizer;
        }

        public void Run()
        {
            var outputFilePath = "output.png";

            var result = visualizer.GetCloudImage()
                .Then(image => image.Save(outputFilePath));

            if (!result.IsSuccess)
            {
                Console.WriteLine(result.Error);
                return;
            }

            Console.WriteLine($"Image save to {Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), outputFilePath)}");
        }
    }
}