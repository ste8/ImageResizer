using CommandLine;
using ImageResizer.Localization;
using ImageResizer.Services;
using Version = ImageResizer.Services.Version;

namespace ImageResizer
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<ResizeOptions>(args)
                    .WithParsed(Main);

                //ATTENTION! This must be here, in order to block the UI
                //and allow the user to read warnings for missing arguments.
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private static void Main(ResizeOptions options)
        {
            var outputWriter = new OutputWriter();

            if (!options.NormalizeAndValidate(outputWriter)) return;

            Console.WriteLine("**********************************");
            Console.WriteLine($"*** Image Resizer v.{Version.GetAssemblyVersion()}");
            Console.WriteLine("**********************************");
            Console.WriteLine("");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("INPUT");
            Console.WriteLine($"- Source: '{options.SourceDirPath}'");
            Console.WriteLine($"- Dest: '{options.DestDirPath}'");
            Console.WriteLine($"- Largest Size: '{options.LargestSize}'");
            Console.WriteLine($"- Required Square: '{options.RequiredSquare}'");
            Console.WriteLine($"- Skip if Exissts: '{options.SkipIfExists}'");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("");

            Console.WriteLine(LocStrings.ResizingImages);
            Console.WriteLine("");
            var resizeService = new ResizeService(outputWriter);
            var resizeCounts = resizeService.ResizeAll(options);

            Console.WriteLine("");
            Console.WriteLine(resizeCounts.Invalid > 0 ? LocStrings.Warning : LocStrings.Done);
            Console.WriteLine(PrintResizeCounts(resizeCounts));
        }

        private static string PrintResizeCounts(ResizeCounts resizeCounts)
        {
            var messages = new List<string>();
            AddPrintResizeCount(LocStrings.Invalid, resizeCounts.Invalid, messages);
            AddPrintResizeCount(LocStrings.Resized, resizeCounts.Resized, messages);
            AddPrintResizeCount(LocStrings.SkippedExisting, resizeCounts.SkippedExisting, messages);
            var result = string.Join(", ", messages);
            return result;
        }

        private static void AddPrintResizeCount(string label, int count, List<string> messages)
        {
            if (count > 0) {
                messages.Add($"{label}: {count}");
            }
        }
    }
}
