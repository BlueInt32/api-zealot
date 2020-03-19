using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public interface IAnnexFileConverter
    {
        OpResult Dump<T>(T node, string basePath) where T : Node;
        OpResult<T> Read<T>(string path) where T : class;
    }
}
