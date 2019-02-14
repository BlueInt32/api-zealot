using Zealot.Domain.Models;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;

namespace Zealot.Services
{
    public interface IProjectService
    {
        OpResult CreateProject(ProjectModel projectModel);
        OpResult UpdateProject(ProjectModel projectModel);
        OpResult<ProjectsConfigsList> ListProjects();
    }
}