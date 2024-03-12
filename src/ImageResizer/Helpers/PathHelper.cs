namespace ImageResizer.Helpers
{
    public class PathHelper
    {
        public static string GetAbsolutePath(string relativePath)
        {
            var assemblyPath = GetBaseDirectoryPath();
            var absolutePath = Path.Combine(assemblyPath, relativePath);
            return absolutePath;
        }

        public static string GetBaseDirectoryPath()
        {
            var assemblyPath = AppContext.BaseDirectory;
            return assemblyPath;

            //var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //string? assemblyPath = Path.GetDirectoryName(assembly.Location);
            //if (assemblyPath == null) {
            //    throw new Exception("Cannot find assembly path.");
            //}
        }
    }
}
