
namespace System.IO
{
    public static class IoHelper
    {
        public static void CreateDirectoryIfNotExists(this string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void CreatFileIfNotExists(this string file)
        {
            if (!File.Exists(file))
            {
                File.Create(file);
            }
        }

    }
}
