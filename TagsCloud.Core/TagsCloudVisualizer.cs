using System.Drawing;
using System.Linq;
using TagsCloud.Core.Settings;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core
{
    public class TagsCloudVisualizer : ITagsCloudVisualizer
    {
        public ITagsCloudCreator TagsCloudCreator { get; }
        public CloudSettings CloudSettings { get; }

        public TagsCloudVisualizer(ITagsCloudCreator tagsCloudCreator, CloudSettings cloudSettings)
        {
            TagsCloudCreator = tagsCloudCreator;
            CloudSettings = cloudSettings;
        }

        public Result<Bitmap> GetCloudImage()
        {
            var imageSettings = CloudSettings.ImageSettings;
            var image = new Bitmap(imageSettings.Width, imageSettings.Height);
            var graphics = Graphics.FromImage(image);

            return TagsCloudCreator
                .CreateTagsCloud(CloudSettings.InputTextFilePath, CloudSettings.FontSettings)
                .Then(tagsCloud => DrawTags(imageSettings, tagsCloud, graphics, CloudSettings.WordsCount))
                .Then(_ => image)
                .RefineError("Failed to create tags cloud image");
        }

        private Result<None> DrawTags(ImageSettings imageSettings, TagsCloud tagsCloud, Graphics graphics, int tagsCount)
        {
            graphics.FillRectangle(new SolidBrush(imageSettings.BackgroundColor), 0, 0, imageSettings.Width, imageSettings.Height);
            var imageSize = new Size(imageSettings.Width, imageSettings.Height);

            foreach (var tag in tagsCloud.Tags.Take(tagsCount))
            {
                var shiftedRectangle = ShiftRectangleToImageCenter(
                    tagsCloud,
                    tag.Rectangle, 
                    imageSize);

                if (!RectangleIsInImageRange(shiftedRectangle, imageSize))
                {
                    return Result.Fail<None>("Tag cloud are out of image, enlarge image size or reduce tags count");
                }

                graphics.DrawString(tag.Text, tag.Font, new SolidBrush(imageSettings.ForegroundColor), shiftedRectangle);
            }

            return Result.Ok();
        }

        private Rectangle ShiftRectangleToImageCenter(TagsCloud tagsCloud, Rectangle rectangle, Size imageSize)
        {
            var newX = rectangle.X + tagsCloud.Center.X + imageSize.Width / 2;
            var newY = rectangle.Y + tagsCloud.Center.Y + imageSize.Height / 2;
            return new Rectangle(newX, newY, rectangle.Width, rectangle.Height);
        }

        private bool RectangleIsInImageRange(Rectangle rectangle, Size imageSize)
        {
            return rectangle.Left >= 0 
                && rectangle.Top >= 0                      
                && rectangle.Right <= imageSize.Width 
                && rectangle.Bottom <= imageSize.Height;
        }
    }
}