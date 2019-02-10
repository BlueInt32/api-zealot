using Zealot.Domain.Models;
using Zealot.Domain.Utilities;
using Zealot.Repository;

namespace Zealot.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public OpResult SaveProject(ProjectModel projectModel)
        {
            return _projectRepository.SaveProject(projectModel);
        }
    }
}
