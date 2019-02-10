namespace SystemWrap
{
    public class DirectoryInfoFactory : IDirectoryInfoFactory
    {
        public IDirectoryInfo Create(string path)
        {
            return new DirectoryInfoWrap(path);
        }
    }
}
