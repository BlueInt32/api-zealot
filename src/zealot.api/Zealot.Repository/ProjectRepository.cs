using System;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Options;
using SystemWrap;
using Zealot.Domain;
using Zealot.Domain.Models;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;
using Zealot.Repository.IO;
using Zealot.Repository.Utils;

namespace Zealot.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ZealotConfiguration _configuration;
        private readonly IDirectoryInfoFactory _directoryInfoFactory;
        private readonly IFileInfoFactory _fileInfoFactory;
        private readonly IJsonFileConverter<Project> _projectFileConverter;
        private readonly IMapper _mapper;
        private readonly IJsonFileConverter<ProjectsConfigsList> _projectsConfigListFileConverter;
        private readonly IAnnexFileConverter _annexFileConverter;

        public ProjectRepository(
            IOptionsMonitor<ZealotConfiguration> zealotConfiguration
            , IDirectoryInfoFactory directoryInfoFactory
            , IFileInfoFactory fileInfoFactory
            , IJsonFileConverter<Project> projectJsonDump
            , IJsonFileConverter<ProjectsConfigsList> projectsConfigListFileConverter
            , IMapper mapper
            , IAnnexFileConverter annexFileConverter)
        {
            _configuration = zealotConfiguration.CurrentValue;
            _directoryInfoFactory = directoryInfoFactory;
            _fileInfoFactory = fileInfoFactory;
            _projectFileConverter = projectJsonDump;
            _mapper = mapper;
            _projectsConfigListFileConverter = projectsConfigListFileConverter;
            _annexFileConverter = annexFileConverter;
        }

        public OpResult CreateProject(ProjectModel model)
        {
            // 1. Check input model
            var directoryInfo = _directoryInfoFactory.Create(model.Path);
            if (!directoryInfo.Exists)
            {
                return OpResult.Bad(ErrorCode.FOLDER_DOES_NOT_EXIST, $"Folder {model.Path} does not exist");
            }

            // 2. Add project to projects list file
            var projectsList = _projectsConfigListFileConverter
                .Read(_configuration.ProjectsListPath)
                .Object;
            if (projectsList.Any(config => config.Path == model.Path))
            {
                return OpResult.Bad(ErrorCode.PROJECT_ALREADY_IN_PROJECT_LIST, $"The configuration already contains a project with the path {model.Path}");
            }
            var projectId = Guid.NewGuid();
            projectsList.Add(new Project
            {
                Id = projectId,
                Name = model.Name,
                Path = Path.Combine(model.Path, _configuration.DefaultProjectFileName)
            });
            _projectsConfigListFileConverter.Dump(projectsList, _configuration.ProjectsListPath);

            // 3. Build domain object
            var project = Project.CreateDefaultInstance();
            project.Id = projectId;
            project.Name = model.Name;
            project.Path = model.Path;

            // 4. Persist the default project file 
            var dumpResult = _projectFileConverter.Dump(project, Path.Combine(model.Path, _configuration.DefaultProjectFileName));

            // 5. Persist the default annex request file
            var rootPack = project.Tree as PackNode;
            if (rootPack != null)
            {
                ProjectTreeHelper<Project>.ExecuteTraversal(
                    rootPack,
                    (node, projectContext) =>
                    {
                        if (!(node is PackNode))
                        {
                            _annexFileConverter.Dump(node, Path.Combine(projectContext.Path, projectContext.Name, $"{node.Id}.yml"));
                        }
                    },
                    project);
            }

            return OpResult.Ok;
        }

        public OpResult<Project> GetProject(Guid projectId)
        {
            var projectsConfigsList = _projectsConfigListFileConverter
                .Read(_configuration.ProjectsListPath)
                .Object;
            var projectConfig = projectsConfigsList.SingleOrDefault(c => c.Id == projectId);
            if (projectConfig == null)
            {
                return OpResult<Project>.Bad(ErrorCode.PROJECT_DOES_NOT_EXIST, $"Project with id {projectId} not found in your configuration");
            }
            var projectResult = _projectFileConverter.Read(projectConfig.Path);

            var rootPackNode = projectResult.Object.Tree as PackNode;
            if (rootPackNode != null)
            {
                for (int i = 0; i < rootPackNode.Children.Count; i++)
                {
                    var recursionContext = new NodeRecursionContext
                    {
                        ChildIndex = i,
                        CurrentNode = rootPackNode.Children[i],
                        ParentNode = rootPackNode,
                        Project = projectConfig

                    };
                    ReadSubTree(recursionContext);
                }
            }
            return projectResult;
        }

        public OpResult<ProjectsConfigsList> ListProjects()
        {
            var list = _projectsConfigListFileConverter.Read(_configuration.ProjectsListPath);
            return list;
        }

        public OpResult UpdateProject(ProjectModel model)
        {
            // 1. Check project exists !
            var projectsConfigsList = _projectsConfigListFileConverter
                .Read(_configuration.ProjectsListPath)
                .Object;
            var projectFromConfig = projectsConfigsList.SingleOrDefault(c => c.Id == model.Id);
            if (projectFromConfig == null)
            {
                return OpResult.Bad(ErrorCode.PROJECT_DOES_NOT_EXIST, $"Project with id {model.Id} not found in your configuration");
            }

            var project = _mapper.Map<Project>(model);

            var rootPack = project.Tree as PackNode;
            if (rootPack != null)
            {
                ProjectTreeHelper<Project>.ExecuteTraversal(
                    rootPack,
                    (node, projectContext) =>
                    {
                        if (!(node is PackNode))
                        {
                            _annexFileConverter.Dump(node, Path.Combine(projectContext.Path, projectContext.Name, $"{node.Id}.yml"));
                        }
                    },
                    project);
            }
            var dumpResult = _projectFileConverter.Dump(project, Path.Combine(project.Path, _configuration.DefaultProjectFileName));

            return dumpResult;
        }

        private void RecursiveAnnexFilesDump(Project project, INode node)
        {
            var pack = node as PackNode;
            if (pack != null)
            {
                foreach (var childNode in pack.Children)
                {
                    RecursiveAnnexFilesDump(project, childNode);
                }
            }
            var request = node as RequestNode;
            if (request != null)
            {
                _annexFileConverter.Dump(request, Path.Combine(project.Path, project.Name, $"{request.Id}.yml"));
            }
        }

        private OpResult ReadSubTree(NodeRecursionContext context)
        {
            if (context.CurrentNode is PackNode)
            {
                var packNode = context.CurrentNode as PackNode;

                for (int i = 0; i < packNode.Children.Count; i++)
                {
                    var recursionContext = new NodeRecursionContext
                    {
                        ChildIndex = i,
                        CurrentNode = packNode.Children[i],
                        ParentNode = packNode,
                        Project = context.Project
                    };
                    ReadSubTree(recursionContext);
                }
            }
            if (context.CurrentNode is RequestNode)
            {
                var requestNode = context.CurrentNode as RequestNode;
                var projectDirectory = _fileInfoFactory.Create(context.Project.Path).Directory.FullName;
                var readResult = _annexFileConverter.Read<RequestNode>(Path.Combine(projectDirectory, requestNode.Id.ToString()) + ".yml");
                if (readResult.Success)
                {
                    (context.ParentNode as PackNode).Children[context.ChildIndex] = readResult.Object;
                }
            }
            return OpResult.Ok;
        }
    }
}
