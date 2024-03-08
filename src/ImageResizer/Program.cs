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
                    .WithParsed(options => {
                        var outputWriter = new OutputWriter();

                        if (!options.NormalizeAndValidate(outputWriter)) {
                            Console.ReadLine();
                            return;
                        }

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
                        resizeService.ResizeAll(options);

                        Console.WriteLine("");
                        Console.WriteLine(LocStrings.Done);
                        Console.ReadLine();
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
