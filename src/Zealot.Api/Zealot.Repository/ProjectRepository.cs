using System.IO;
using SystemWrap;
using Zealot.Domain;
using Zealot.Domain.Models;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;
using Zealot.Repository.IO;

namespace Zealot.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDirectoryInfoFactory _directoryInfoFactory;
        private readonly IObjectJsonDump<Project> _projectJsonDump;

        public ProjectRepository(
            IDirectoryInfoFactory directoryInfoFactory
            , IObjectJsonDump<Project> projectJsonDump)
        {
            _directoryInfoFactory = directoryInfoFactory;
            _projectJsonDump = projectJsonDump;
        }
        public OpResult SaveProject(ProjectModel model)
        {
            // 1. checks on input model
            var directoryInfo = _directoryInfoFactory.Create(model.Folder);
            if (!directoryInfo.Exists)
            {
                return OpResult.Bad(Constants.FOLDER_DOES_NOT_EXISTS, $"Folder {model.Folder} does not exist");
            }

            // 2. build domain object
            var project = Project.CreateDefaultInstance();
            project.Name = model.Name;
            project.Folder = model.Folder;

            // 3. persist in fileSystem right away
            var dumpResult = _projectJsonDump.Dump(project, Path.Combine(project.Folder, "project.json"));

            return OpResult.Ok;
        }
    }
}
