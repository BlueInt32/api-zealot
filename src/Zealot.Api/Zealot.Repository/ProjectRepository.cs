using System;
using System.IO;
using System.Linq;
using AutoMapper;
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
        private readonly string _configsPath = @"D:\_Prog\Projects\Zealot\test\projects.json";
        private readonly IDirectoryInfoFactory _directoryInfoFactory;
        private readonly IJsonFileConverter<Project> _projectFileConverter;
        private readonly IMapper _mapper;
        private readonly IJsonFileConverter<ProjectsConfigsList> _projectsConfigListFileConverter;

        public ProjectRepository(
            IDirectoryInfoFactory directoryInfoFactory
            , IJsonFileConverter<Project> projectJsonDump
            , IJsonFileConverter<ProjectsConfigsList> projectsConfigListFileConverter
            , IMapper mapper)
        {
            _directoryInfoFactory = directoryInfoFactory;
            _projectFileConverter = projectJsonDump;
            _mapper = mapper;
            _projectsConfigListFileConverter = projectsConfigListFileConverter;
        }
        public OpResult CreateProject(ProjectModel model)
        {
            // 1. checks on input model
            var directoryInfo = _directoryInfoFactory.Create(model.Path);
            if (!directoryInfo.Exists)
            {
                return OpResult.Bad(Constants.FOLDER_DOES_NOT_EXISTS, $"Folder {model.Path} does not exist");
            }

            // 2. Add project to main reference file
            var projectsConfigsList = _projectsConfigListFileConverter
                .Read(_configsPath)
                .Object;
            if (projectsConfigsList.Any(config => config.Path == model.Path))
            {
                return OpResult.Bad(Constants.PROJECT_ALREADY_IN_PROJECT_LIST, $"The configuration already contains a project with the path {model.Path}");
            }
            var projectId = Guid.NewGuid();
            projectsConfigsList.Add(new ProjectConfig { Id = projectId, Path = model.Path });
            _projectsConfigListFileConverter.Dump(projectsConfigsList, _configsPath);

            // 3. build domain object
            var project = Project.CreateDefaultInstance();
            project.Id = projectId;
            project.Name = model.Name;

            // 4. persist in fileSystem right away
            var dumpResult = _projectFileConverter.Dump(project, Path.Combine(model.Path, "project.json"));

            return OpResult.Ok;
        }
        public OpResult UpdateProject(ProjectModel model)
        {
            // 1. Check project exists !
            var projectsConfigsList = _projectsConfigListFileConverter
                .Read(_configsPath)
                .Object;
            var projectConfig = projectsConfigsList.SingleOrDefault(c => c.Id == model.Id);
            if (projectConfig == null)
            {
                return OpResult.Bad(Constants.PROJECT_DOES_NOT_EXIST, $"Project with id {model.Id} not found in your configuration");
            }

            var project = _mapper.Map<Project>(model);
            var dumpResult = _projectFileConverter.Dump(project, Path.Combine(projectConfig.Path, "project.json"));

            return dumpResult;
        }
    }
}
