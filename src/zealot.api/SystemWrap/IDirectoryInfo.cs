namespace SystemWrap
{
    public interface IDirectoryInfo
    {
        bool Exists { get; }
        string FullName { get; }
    }
}
