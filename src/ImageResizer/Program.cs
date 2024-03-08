using CommandLine;

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

                        Console.WriteLine($"Source: '{options.SourceDirPath}'");
                        Console.WriteLine($"Dest: '{options.DestDirPath}'");
                        Console.WriteLine($"Largest size: '{options.LargestSize}'");
                        Console.WriteLine($"Required Square: '{options.RequiredSquare}'");


                    });

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
