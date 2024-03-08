using CommandLine;
using ImageResizer.Helpers;

namespace ImageResizer
{
    public class ResizeOptions
    {
        [Option("source", Required = true, HelpText = "Source directory")]
        public string SourceDirPath { get; set; } = string.Empty;

        [Option("dest", Required = true, HelpText = "Destination directory")]
        public string DestDirPath { get; set; } = string.Empty;

        [Option("size", Required = true, HelpText = "Largest size (in pixel)")]
        public int LargestSize { get; set; }

        [Option("square", Required = true, HelpText = "Required square (true/false)")]
        public bool RequiredSquare { get; set; }

        public bool NormalizeAndValidate(IOutputWriter outputWriter)
        {
            var dirPath = PathHelper.GetAbsolutePath(SourceDirPath);
            if (!Directory.Exists(dirPath)) {
                outputWriter.WriteLine($"Source Directory does not exists: '{SourceDirPath}'");
                return false;
            }
            SourceDirPath = dirPath;

            dirPath = PathHelper.GetAbsolutePath(DestDirPath);
            if (!Directory.Exists(dirPath)) {
                outputWriter.WriteLine($"Destination Directory does not exists: '{DestDirPath}'");
                return false;
            }
            DestDirPath = dirPath;

            return true;
        }
    }
}
