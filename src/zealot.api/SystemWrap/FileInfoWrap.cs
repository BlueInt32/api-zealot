using System.IO;

namespace SystemWrap
{
    public class FileInfoWrap : IFileInfo
    {
        public FileInfo FileInfo { get; private set; }

        public FileInfoWrap(string path)
        {
            FileInfo = new FileInfo(path);
        }
        public IDirectoryInfo Directory => new DirectoryInfoWrap(FileInfo.Directory);
    }
}
