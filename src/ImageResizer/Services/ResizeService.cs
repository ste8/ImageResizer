using ImageResizer.Localization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using ResizeOptions = ImageResizer.Localization.ResizeOptions;

namespace ImageResizer.Services
{
    public class ResizeService
    {
        private readonly OutputWriter _outputWriter;

        public ResizeService(OutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void ResizeAll(ResizeOptions options)
        {
            var sourceImagesPath = GetSourceImagesPath(options.SourceDirPath);
            foreach (var sourceImagePath in sourceImagesPath)
            {
                var result = ResizeImage(sourceImagePath, options);
                if (result.Result != ImageResizeResultType.Resized) {
                    WriteOutputForImage(result.Message);
                }
            }
        }

        private string[] GetSourceImagesPath(string sourceDirPath)
        {
            var allowedExtensions = new[] { ".jpg", ".png" };
            var files = Directory
                .GetFiles(sourceDirPath)
                .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                .ToArray();
            return files;
        }

        public static ImageResizeResult ResizeImage(string sourceImageFilePath, ResizeOptions options)
        {
            var imageFileName = Path.GetFileName(sourceImageFilePath);
            using var image = Image.Load(sourceImageFilePath);

            var validationResult = ValidateResizeImage(image, imageFileName, options);
            if (validationResult != null) return validationResult;

            var newSize = CalculateNewSize(image, options);
            ResizeToNewSize(image, newSize);
            SaveToDestPath(options, imageFileName, image);
            return new ImageResizeResult(ImageResizeResultType.Resized);
        }

        private static ImageResizeResult? ValidateResizeImage(Image image, string imageFileName, ResizeOptions options)
        {
            if (IsImageSmallerThanRequiredSize(image, options)) {
                var message = LocStrings.ImageIsSmallerThanRequiredSize(imageFileName, options.LargestSize);
                return new ImageResizeResult(ImageResizeResultType.SmallerThanRequiredSize, message);
            }
            if (options.RequiredSquare) {
                if (!IsImageSquare(image)) {
                    var message = LocStrings.ImageIsNotSquare(imageFileName, image.Width, image.Height);
                    return new ImageResizeResult(ImageResizeResultType.NotSquare, message);
                }
            }
            return null;
        }

        private static Size CalculateNewSize(Image image, ResizeOptions options)
        {
            decimal aspectRatio = (decimal)image.Width / image.Height;
            int newWidth;
            int newHeight;
            if (image.Width > image.Height) {
                newWidth = options.LargestSize;
                newHeight = Convert.ToInt32(newWidth / aspectRatio);
            } else {
                newHeight = options.LargestSize;
                newWidth = Convert.ToInt32(newHeight * aspectRatio);
            }
            return new Size(newWidth, newHeight);
        }

        private static void SaveToDestPath(ResizeOptions options, string imageFileName, Image image)
        {
            var destImageFilePath = Path.Combine(options.DestDirPath, imageFileName);
            image.Save(destImageFilePath);
        }

        private static void ResizeToNewSize(Image image, Size newSize)
        {
            image.Mutate(x => x.Resize(newSize));
        }

        private void WriteOutputForImage(string? text)
        {
            _outputWriter.WriteLine(text ?? "Message not defined");
        }

        private static bool IsImageSmallerThanRequiredSize(Image image, ResizeOptions options)
        {
            bool result = image.Width <= options.LargestSize
                          && image.Height <= options.LargestSize;
            return result;
        }

        private static bool IsImageSquare(Image image)
        {
            bool result = image.Width == image.Height;
            return result;
        }
    }

    public record ImageResizeResult(ImageResizeResultType Result, string? Message = null)
    {
       
    }

    public enum ImageResizeResultType
    {
        Undefined,
        Resized,
        NotSquare,
        SkippedExisting,
        SmallerThanRequiredSize
    }
}
