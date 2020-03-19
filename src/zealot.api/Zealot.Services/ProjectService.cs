using System;
using System.Collections.Generic;
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
        public OpResult CreateProject(Project inputProject)
        {
            return _projectRepository.CreateProject(inputProject);
        }

        public OpResult<Project> GetProject(Guid projectId)
        {
            return _projectRepository.GetProject(projectId);
        }

        public OpResult<List<Project>> ListProjects()
        {
            return _projectRepository.ListProjects();
        }

        public OpResult UpdateProject(Project inputProject)
        {
            return _projectRepository.UpdateProject(inputProject);
        }

    }
}
