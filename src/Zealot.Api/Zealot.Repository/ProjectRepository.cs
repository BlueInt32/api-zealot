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
        private readonly IAnnexFileConverter _annexFileConverter;

        public ProjectRepository(
            IDirectoryInfoFactory directoryInfoFactory
            , IJsonFileConverter<Project> projectJsonDump
            , IJsonFileConverter<ProjectsConfigsList> projectsConfigListFileConverter
            , IMapper mapper
            , IAnnexFileConverter annexFileConverter)
        {
            _directoryInfoFactory = directoryInfoFactory;
            _projectFileConverter = projectJsonDump;
            _mapper = mapper;
            _projectsConfigListFileConverter = projectsConfigListFileConverter;
            _annexFileConverter = annexFileConverter;
        }
        public OpResult CreateProject(ProjectModel model)
        {
            // 1. checks on input model
            var directoryInfo = _directoryInfoFactory.Create(model.Path);
            if (!directoryInfo.Exists)
            {
                return OpResult.Bad(ErrorCode.FOLDER_DOES_NOT_EXIST, $"Folder {model.Path} does not exist");
            }

            // 2. Add project to main reference file
            var projectsConfigsList = _projectsConfigListFileConverter
                .Read(_configsPath)
                .Object;
            if (projectsConfigsList.Any(config => config.Path == model.Path))
            {
                return OpResult.Bad(ErrorCode.PROJECT_ALREADY_IN_PROJECT_LIST, $"The configuration already contains a project with the path {model.Path}");
            }
            var projectId = Guid.NewGuid();
            projectsConfigsList.Add(new ProjectConfig { Id = projectId, Path = Path.Combine(model.Path, "project.json") });
            _projectsConfigListFileConverter.Dump(projectsConfigsList, _configsPath);

            // 3. build domain object
            var project = Project.CreateDefaultInstance();
            project.Id = projectId;
            project.Name = model.Name;

            // 4. persist in fileSystem right away
            var dumpResult = _projectFileConverter.Dump(project, Path.Combine(model.Path, "project.json"));

            return OpResult.Ok;
        }

        public OpResult<Project> GetProject(Guid projectId)
        {
            var projectsConfigsList = _projectsConfigListFileConverter
                .Read(_configsPath)
                .Object;
            var projectConfig = projectsConfigsList.SingleOrDefault(c => c.Id == projectId);
            if (projectConfig == null)
            {
                return OpResult<Project>.Bad(ErrorCode.PROJECT_DOES_NOT_EXIST, $"Project with id {projectId} not found in your configuration");
            }
            var projectResult = _projectFileConverter.Read(projectConfig.Path);
            return projectResult;
        }

        public OpResult<ProjectsConfigsList> ListProjects()
        {
            var list = _projectsConfigListFileConverter.Read(_configsPath);
            return list;
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
                return OpResult.Bad(ErrorCode.PROJECT_DOES_NOT_EXIST, $"Project with id {model.Id} not found in your configuration");
            }

            var project = _mapper.Map<Project>(model);

            RecursiveAnnexFilesDump(project.Tree);
            var dumpResult = _projectFileConverter.Dump(project, projectConfig.Path);

            return dumpResult;
        }

        private void RecursiveAnnexFilesDump(SubTree tree)
        {
            _annexFileConverter.Dump(tree, @"D:\_Prog\Projects\Zealot\test\folder1");
            if (tree.Children != null)
            {
                foreach (var child in tree.Children)
                {
                    RecursiveAnnexFilesDump(child);
                }
            }
        }
    }
}
