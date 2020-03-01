using Zealot.Domain;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public interface IRequestSerializer
    {
        OpResult<string> Serialize(SubTree subtree);
        OpResult<SubTree> Deserialize(string input);
    }
}
