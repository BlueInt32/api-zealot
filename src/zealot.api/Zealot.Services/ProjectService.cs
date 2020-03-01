using System;
using Zealot.Domain.Models;
using Zealot.Domain.Objects;
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
        public OpResult CreateProject(ProjectModel projectModel)
        {
            return _projectRepository.CreateProject(projectModel);
        }

        public OpResult<Project> GetProject(Guid projectId)
        {
            return _projectRepository.GetProject(projectId);
        }

        public OpResult<ProjectsConfigsList> ListProjects()
        {
            return _projectRepository.ListProjects();
        }

        public OpResult UpdateProject(ProjectModel projectModel)
        {
            return _projectRepository.UpdateProject(projectModel);
        }

    }
}
