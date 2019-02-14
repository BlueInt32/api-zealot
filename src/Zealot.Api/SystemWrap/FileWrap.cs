using System.IO;
using System.Text;

namespace SystemWrap
{
    public class FileWrap : IFile
    {
        public void WriteAllBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        public void WriteAllText(string path, string text, Encoding encoding)
        {
            File.WriteAllText(path, text, encoding);
        }

        public string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path, encoding);
        }
    }
}
