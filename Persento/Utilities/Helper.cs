using System.IO;

namespace Persento.Utilities
{
    public static class Helper
    {
        public static void RemoveFile(string root,string folder,string image)
        {
            var photo = Path.Combine(root, folder, image);
            if (File.Exists(photo))
            {
                File.Delete(photo);
            }
        }
    }
}
