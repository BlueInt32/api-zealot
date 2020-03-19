using System;
using System.Collections.Generic;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;

namespace Zealot.Services
{
    public interface IProjectService
    {
        OpResult CreateProject(Project inputProject);
        OpResult UpdateProject(Project inputProject);
        OpResult<List<Project>> ListProjects();
        OpResult<Project> GetProject(Guid projectId);
    }
}