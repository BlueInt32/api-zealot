using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using SystemWrap;
using Zealot.Domain;
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
        private readonly IJsonFileConverter<List<Project>> _projectsConfigListFileConverter;
        private readonly IAnnexFileConverter _annexFileConverter;

        public ProjectRepository(
            IOptionsMonitor<ZealotConfiguration> zealotConfiguration
            , IDirectoryInfoFactory directoryInfoFactory
            , IFileInfoFactory fileInfoFactory
            , IJsonFileConverter<Project> projectJsonDump
            , IJsonFileConverter<List<Project>> projectsConfigListFileConverter
            , IAnnexFileConverter annexFileConverter)
        {
            _configuration = zealotConfiguration.CurrentValue;
            _directoryInfoFactory = directoryInfoFactory;
            _fileInfoFactory = fileInfoFactory;
            _projectFileConverter = projectJsonDump;
            _projectsConfigListFileConverter = projectsConfigListFileConverter;
            _annexFileConverter = annexFileConverter;
        }

        public OpResult CreateProject(Project inputProject)
        {
            // 1. Check input model
            var directoryInfo = _directoryInfoFactory.Create(inputProject.Path);
            if (!directoryInfo.Exists)
            {
                return OpResult.Bad(ErrorCode.FOLDER_DOES_NOT_EXIST, $"Folder {inputProject.Path} does not exist");
            }

            // 2. Add project to projects list file
            var projectsList = _projectsConfigListFileConverter
                .Read(_configuration.ProjectsListPath)
                .Object;
            if (projectsList.Any(config => config.Path == inputProject.Path))
            {
                return OpResult.Bad(ErrorCode.PROJECT_ALREADY_IN_PROJECT_LIST, $"The configuration already contains a project with the path {inputProject.Path}");
            }
            var projectId = Guid.NewGuid();
            projectsList.Add(new Project
            {
                Id = projectId,
                Name = inputProject.Name,
                Path = Path.Combine(inputProject.Path, _configuration.DefaultProjectFileName)
            });
            _projectsConfigListFileConverter.Dump(projectsList, _configuration.ProjectsListPath);

            // 3. Build domain object
            var project = Project.CreateDefaultInstance();
            project.Id = projectId;
            project.Name = inputProject.Name;
            project.Path = inputProject.Path;

            // 4. Persist the default project file 
            var dumpResult = _projectFileConverter.Dump(project, Path.Combine(inputProject.Path, _configuration.DefaultProjectFileName));

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
            var projectResult = _projectFileConverter.Read(Path.Combine(projectConfig.Path, _configuration.DefaultProjectFileName));

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

        public OpResult<List<Project>> ListProjects()
        {
            var list = _projectsConfigListFileConverter.Read(_configuration.ProjectsListPath);
            return list;
        }

        public OpResult UpdateProject(Project inputProject)
        {
            // 1. Check project exists !
            var projectsConfigsList = _projectsConfigListFileConverter
                .Read(_configuration.ProjectsListPath)
                .Object;
            var projectFromConfig = projectsConfigsList.SingleOrDefault(c => c.Id == inputProject.Id);
            if (projectFromConfig == null)
            {
                return OpResult.Bad(ErrorCode.PROJECT_DOES_NOT_EXIST, $"Project with id {inputProject.Id} not found in your configuration");
            }

            inputProject.Path = projectFromConfig.Path;

            var rootPack = inputProject.Tree as PackNode;
            if (rootPack != null)
            {
                ProjectTreeHelper<Project>.ExecuteTraversal(
                    rootPack,
                    (node, projectContext) =>
                    {
                        if (!(node is PackNode))
                        {
                            _annexFileConverter.Dump(node, Path.Combine(projectContext.Path, $"{node.Id}.yml"));
                        }
                    },
                    inputProject);
            }
            var dumpResult = _projectFileConverter.Dump(inputProject, Path.Combine(inputProject.Path, _configuration.DefaultProjectFileName));

            return dumpResult;
        }

        private void RecursiveAnnexFilesDump(Project project, Node node)
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
                var readResult = _annexFileConverter.Read<RequestNode>(Path.Combine(context.Project.Path, requestNode.Id.ToString()) + ".yml");
                if (readResult.Success)
                {
                    (context.ParentNode as PackNode).Children[context.ChildIndex] = readResult.Object;
                }
            }
            return OpResult.Ok;
        }
    }
}
