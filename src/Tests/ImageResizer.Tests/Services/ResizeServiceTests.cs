using ImageResizer.Helpers;
using ImageResizer.Localization;
using ImageResizer.Services;
using NUnit.Framework;
using SixLabors.ImageSharp;

namespace ImageResizer.Tests.Services
{
    [TestFixture]
    public class ResizeServiceTests
    {
        private readonly string _sourceDirPath = PathHelper.GetAbsolutePath(@"Services\Images\");
        private readonly string _destDirPath = PathHelper.GetAbsolutePath(@"Services\Images-Dest");

        #region Infrastructure

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(_destDirPath)) {
                Directory.Delete(_destDirPath, true);
            }
            Directory.CreateDirectory(_destDirPath);
        }

        private void AssertDestImageSize(string imageFileName, int expectedWidth, int expectedHeight)
        {
            var destFilePath = Path.Combine(_destDirPath, imageFileName);
            using var image = Image.Load(destFilePath);
            Assert.That(image.Width, Is.EqualTo(expectedWidth));
            Assert.That(image.Height, Is.EqualTo(expectedHeight));
        }

        private void AssertDestImageNotExists(string imageFileName)
        {
            var destFilePath = Path.Combine(_destDirPath, imageFileName);
            Assert.That(File.Exists(destFilePath), Is.False);
        }

        #endregion

        [Test]
        public void Resize_WhenSquareJpg_CreateJpg()
        {
            var imageFileName = "Image-Square-1024x1024-Jpg.jpg";
            var sourcePath = Path.Combine(_sourceDirPath, imageFileName);
            var options = new ResizeOptions {
                SourceDirPath = _sourceDirPath,
                DestDirPath = _destDirPath,
                LargestSize = 250,
                RequiredSquare = true
            };
            var result = ResizeService.ResizeImage(sourcePath, options);

            Assert.That(result.Result, Is.EqualTo(ImageResizeResultType.Resized));
            AssertDestImageSize(imageFileName, 250, 250);
        }

        [TestCase(250)]
        [TestCase(1023)]
        public void Resize_WhenSquareTransparentPng_CreateTransparentPng(int largestSize)
        {
            var imageFileName = "Image-Square-1024x1024-Png.png";
            var sourcePath = Path.Combine(_sourceDirPath, imageFileName);
            var options = new ResizeOptions {
                SourceDirPath = _sourceDirPath,
                DestDirPath = _destDirPath,
                LargestSize = largestSize,
                RequiredSquare = true
            };
            var result = ResizeService.ResizeImage(sourcePath, options);

            Assert.That(result.Result, Is.EqualTo(ImageResizeResultType.Resized));
            AssertDestImageSize(imageFileName, largestSize, largestSize);
        }

        [TestCase(200)]
        [TestCase(250)]
        public void Resize_WhenSquareSmallerThanRequestedSize_SkipCreation(int largestSize)
        {
            var imageFileName = "Image-Square-200x200.jpg";
            var sourcePath = Path.Combine(_sourceDirPath, imageFileName);
            var options = new ResizeOptions {
                SourceDirPath = _sourceDirPath,
                DestDirPath = _destDirPath,
                LargestSize = largestSize,
                RequiredSquare = true
            };
            var result = ResizeService.ResizeImage(sourcePath, options);

            Assert.That(result.Result, Is.EqualTo(ImageResizeResultType.SmallerThanRequiredSize));
            AssertDestImageNotExists(imageFileName);
        }


        [Test]
        public void Resize_WhenRectAndSquareIsRequired_ReturnError()
        {
            var imageFileName = "Image-Rect-1024x709.jpg";
            var sourcePath = Path.Combine(_sourceDirPath, imageFileName);
            var options = new ResizeOptions {
                SourceDirPath = _sourceDirPath,
                DestDirPath = _destDirPath,
                LargestSize = 250,
                RequiredSquare = true
            };
            var result = ResizeService.ResizeImage(sourcePath, options);
            Assert.That(result.Result, Is.EqualTo(ImageResizeResultType.NotSquare));
        }

        [TestCase(200, 200, 138)]
        [TestCase(250, 250, 173)]
        public void Resize_WhenRectAndWidthGreatedThanHeight(int largestSize, int expectedWidth, int expectedHeight)
        {
            var imageFileName = "Image-Rect-1024x709.jpg";
            var sourcePath = Path.Combine(_sourceDirPath, imageFileName);
            var options = new ResizeOptions {
                SourceDirPath = _sourceDirPath,
                DestDirPath = _destDirPath,
                LargestSize = largestSize,
                RequiredSquare = false
            };
            var result = ResizeService.ResizeImage(sourcePath, options);

            Assert.That(result.Result, Is.EqualTo(ImageResizeResultType.Resized));
            AssertDestImageSize(imageFileName, expectedWidth, expectedHeight);
        }

        [TestCase(200, 148, 200)]
        [TestCase(250, 185, 250)]
        public void Resize_WhenRectAndWidthLessThanHeight(int largestSize, int expectedWidth, int expectedHeight)
        {
            var imageFileName = "Image-Rect-759x1024.png";
            var sourcePath = Path.Combine(_sourceDirPath, imageFileName);
            var options = new ResizeOptions {
                SourceDirPath = _sourceDirPath,
                DestDirPath = _destDirPath,
                LargestSize = largestSize,
                RequiredSquare = false
            };
            var result = ResizeService.ResizeImage(sourcePath, options);

            Assert.That(result.Result, Is.EqualTo(ImageResizeResultType.Resized));
            AssertDestImageSize(imageFileName, expectedWidth, expectedHeight);
        }

        [TestCase("Image-Rect-1024x709.jpg", 1024)]
        [TestCase("Image-Rect-1024x709.jpg", 2000)]
        [TestCase("Image-Rect-759x1024.png", 1024)]
        [TestCase("Image-Rect-759x1024.png", 2000)]
        public void Resize_WhenRectAnSmallerThanRequiredSize(string imageFileName, int largestSize)
        {
            var sourcePath = Path.Combine(_sourceDirPath, imageFileName);
            var options = new ResizeOptions
            {
                SourceDirPath = _sourceDirPath,
                DestDirPath = _destDirPath,
                LargestSize = largestSize,
                RequiredSquare = false
            };
            var result = ResizeService.ResizeImage(sourcePath, options);

            Assert.That(result.Result, Is.EqualTo(ImageResizeResultType.SmallerThanRequiredSize));
        }
    }
}
