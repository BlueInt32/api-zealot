using System;
using System.Collections.Generic;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;

namespace Zealot.Repository
{
    public interface IProjectRepository
    {
        OpResult CreateProject(Project inputProject);
        OpResult UpdateProject(Project inputProject);
        OpResult<List<Project>> ListProjects();
        OpResult<Project> GetProject(Guid projectId);
    }
}