using CommandLine;
using ImageResizer.Helpers;
using ImageResizer.Services;

namespace ImageResizer.Localization
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

        [Option("skip", Required = false, HelpText = "Skip existing (true/false)")]
        public bool SkipIfExists { get; set; }

        public bool NormalizeAndValidate(OutputWriter outputWriter)
        {
            var dirPath = PathHelper.GetAbsolutePath(SourceDirPath);
            if (!Directory.Exists(dirPath)) {
                outputWriter.WriteLine($"Source Directory does not exists: '{dirPath}'");
                return false;
            }
            SourceDirPath = dirPath;

            dirPath = PathHelper.GetAbsolutePath(DestDirPath);
            if (!Directory.Exists(dirPath)) {
                outputWriter.WriteLine($"Destination Directory does not exists: '{dirPath}'");
                return false;
            }
            DestDirPath = dirPath;

            return true;
        }
    }
}
