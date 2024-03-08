namespace ImageResizer.Services
{
    public class Version
    {
        public static string GetAssemblyVersion()
        {
            var version = typeof(Version).Assembly.GetName().Version;
            var result = version?.ToString(3);
            return result ?? string.Empty;
        }
    }
}
