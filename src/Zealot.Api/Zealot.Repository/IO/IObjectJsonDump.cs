using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public interface IObjectJsonDump<T>
    {
        OpResult Dump(T inputObject, string path);
    }
}