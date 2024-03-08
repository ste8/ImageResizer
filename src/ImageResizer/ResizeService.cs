using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer
{
    public class ResizeService
    {
        private readonly IOutputWriter _outputWriter;

        public ResizeService(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void ResizeAll(ResizeOptions options)
        {
            var sourceImagesPath = GetSourceImagesPath(options.SourceDirPath);
            foreach (var sourceImagePath in sourceImagesPath)
            {
                ResizeImage(sourceImagePath, options);
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

        private void ResizeImage(string sourceImageFilePath, ResizeOptions options)
        {
            var imageFileName = Path.GetFileName(sourceImageFilePath);
            using var image = Image.Load(sourceImageFilePath);
            if (IsImageSmallerThanRequiredSize(image, options)) {
                WriteOutputForImage($"Image '{imageFileName}' is already smaller than {options.LargestSize} pixel.");
                return;
            }
            if (options.RequiredSquare) {
                if (IsImageSquare(image)) {
                    WriteOutputForImage($"Image '{imageFileName}' is not square. Size: ({image.Width}x{image.Height}).");
                    return;
                }
            }
            
            var aspectRatio = image.Width / image.Height;
            int newWidth;
            int newHeight;
            if (image.Width > image.Height) {
                newWidth = options.LargestSize;
                newHeight = newWidth / aspectRatio;
            } else {
                newHeight = options.LargestSize;
                newWidth = newHeight * aspectRatio;
            }

            image.Mutate(x =>
                x.Resize(new SixLabors.ImageSharp.Processing.ResizeOptions() {
                        Mode = ResizeMode.Manual,
                        Size = new Size(newWidth, newHeight)
                    }
                )
            );
            var destImageFilePath = Path.Combine(options.DestDirPath, imageFileName);
            image.Save(destImageFilePath);
        }

        private void WriteOutputForImage(string text)
        {
            _outputWriter.WriteLine(text);
        }

        private bool IsImageSmallerThanRequiredSize(Image image, ResizeOptions options)
        {
            bool result = image.Width < options.LargestSize
                          && image.Height < options.LargestSize;
            return result;
        }

        private bool IsImageSquare(Image image)
        {
            bool result = image.Width == image.Height;
            return result;
        }


    }
}
