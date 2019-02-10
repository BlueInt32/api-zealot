using Zealot.Domain.Models;
using Zealot.Domain.Utilities;

namespace Zealot.Services
{
    public interface IProjectService
    {
        OpResult SaveProject(ProjectModel projectModel);
    }
}