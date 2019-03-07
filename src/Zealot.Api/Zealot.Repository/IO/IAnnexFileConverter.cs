using Zealot.Domain;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public interface IAnnexFileConverter
    {
        OpResult Dump(SubTree subTree, string basePath);
    }
}
