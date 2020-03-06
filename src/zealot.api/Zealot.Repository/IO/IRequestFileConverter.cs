using Zealot.Domain;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public interface IRequestFileConverter
    {
        OpResult Dump(Request subTree, string basePath);
        OpResult<Request> Read(string path);
    }
}
