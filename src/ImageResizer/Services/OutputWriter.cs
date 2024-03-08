namespace ImageResizer.Services
{
    public interface IOutputWriter
    {
        public void WriteLine(string text);
    }

    public class OutputWriter : IOutputWriter
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
