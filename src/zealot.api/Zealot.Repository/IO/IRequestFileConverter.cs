using Zealot.Domain;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public interface IRequestFileConverter
    {
        OpResult Dump(RequestNode request, string basePath);
        OpResult<RequestNode> Read(string path);
    }
}
