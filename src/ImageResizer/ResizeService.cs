﻿using SixLabors.ImageSharp;

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

        private void ResizeImage(string sourceImagePath, ResizeOptions options)
        {
            using var image = Image.Load(sourceImagePath);
            if (IsImageSmallerThanRequiredSize(image, options))
            {
                _outputWriter.WriteLine("");
                return;
            }
        }

        private bool IsImageSmallerThanRequiredSize(Image image, ResizeOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
