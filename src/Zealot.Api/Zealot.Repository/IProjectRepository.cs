using Zealot.Domain.Models;
using Zealot.Domain.Utilities;

namespace Zealot.Repository
{
    public interface IProjectRepository
    {
        OpResult SaveProject(ProjectModel model);
    }
}