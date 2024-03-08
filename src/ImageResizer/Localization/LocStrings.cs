namespace ImageResizer.Localization
{
    public class LocStrings
    {
        public static string ResizingImages = "Resizing images...";

        public static string Done = "DONE!";

        public static string ImageIsSmallerThanRequiredSize(string imageFileName, int largestSize)
        {
            return $"Image '{imageFileName}' is already smaller than {largestSize} pixel.";
        }

        public static string ImageIsNotSquare(string imageFileName, int width, int height)
        {
            return $"Image '{imageFileName}' is not square. Size: ({width}x{height})";
        }
    }
}
