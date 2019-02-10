using System.IO;

namespace SystemWrap
{
    public class DirectoryInfoWrap : IDirectoryInfo
    {
        public DirectoryInfo DirectoryInfo { get; private set; }

        public bool Exists
        {
            get { return DirectoryInfo.Exists; }
        }



        public DirectoryInfoWrap(string path)
        {
            DirectoryInfo = new DirectoryInfo(path);
        }
    }
}
