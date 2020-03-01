using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public interface IJsonFileConverter<T> where T : class
    {
        OpResult<T> Read(string path);
        OpResult Dump(T inputObject, string path);
    }
}