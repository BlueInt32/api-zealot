namespace SystemWrap
{
    public class FileInfoFactory : IFileInfoFactory
    {
        IFileInfo IFileInfoFactory.Create(string path)
        {
            return new FileInfoWrap(path);
        }
    }
}
