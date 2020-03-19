using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemWrap;
using Zealot.Domain;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;
using Zealot.Repository.IO;

namespace Zealot.Repository.Tests
{
    [TestClass]
    public class ProjectRepositoryTest
    {
        private Mock<IDirectoryInfoFactory> _directoryInfoFactoryMock;
        private Mock<IFileInfoFactory> _fileInfoFactoryMock;
        private Mock<IJsonFileConverter<Project>> _projectFileConverterMock;
        private Mock<IJsonFileConverter<ProjectsConfigsList>> _projectConfigsListFileConverterMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAnnexFileConverter> _annexFileConverterMock;
        private Mock<IOptionsMonitor<ZealotConfiguration>> _zealotConfigurationMock;

        private ProjectRepository _repository;

        private Guid projectId = Guid.NewGuid();
        private string _projectsListFilePath = @"E:\projects\list.json";
        private string _projectPath = "any folder";
        private string _defaultProjectFileName = "coucou.txt";
        private string _guidRegex = "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})";


        [TestInitialize]
        public void TestInitialize()
        {
            _zealotConfigurationMock = new Mock<IOptionsMonitor<ZealotConfiguration>>();
            _directoryInfoFactoryMock = new Mock<IDirectoryInfoFactory>();
            _fileInfoFactoryMock = new Mock<IFileInfoFactory>();
            _projectFileConverterMock = new Mock<IJsonFileConverter<Project>>();
            _projectConfigsListFileConverterMock = new Mock<IJsonFileConverter<ProjectsConfigsList>>();
            _mapperMock = new Mock<IMapper>();
            _annexFileConverterMock = new Mock<IAnnexFileConverter>();

            _zealotConfigurationMock.Setup(m => m.CurrentValue).Returns(new ZealotConfiguration
            {
                ProjectsListPath = _projectsListFilePath,
                DefaultProjectFileName = _defaultProjectFileName
            });

            _repository = new ProjectRepository(
                _zealotConfigurationMock.Object
                , _directoryInfoFactoryMock.Object
                , _fileInfoFactoryMock.Object
                , _projectFileConverterMock.Object
                , _projectConfigsListFileConverterMock.Object
                , _annexFileConverterMock.Object
                );

            _projectConfigsListFileConverterMock
                .Setup(m => m.Read(_projectsListFilePath))
                .Returns(new OpResult<ProjectsConfigsList>
                {
                    Object = new ProjectsConfigsList
                    {
                        new Project
                        {
                            Id = projectId,
                            Path = _projectPath
                        }
                    }
                });
        }

        [TestMethod]
        public void CreateProject_ShouldReadProjectsList_AddTheProjectToTheList_DumpTheList()
        {
            // arrange
            var projectPath = "any path";
            var model = new Project
            {
                Name = "project Name",
                Path = projectPath
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(true);
            _directoryInfoFactoryMock.Setup(m => m.Create(projectPath)).Returns(directoryInfoMock.Object);

            // act
            var opResult = _repository.CreateProject(model);

            // assert
            _projectConfigsListFileConverterMock
                .Verify(m => m.Read(_projectsListFilePath), Times.Once);
            _projectConfigsListFileConverterMock
                .Verify(m => m.Dump(It.Is<ProjectsConfigsList>(l =>
                    l.Count(p => p.Name == "project Name" && p.Path == $@"{projectPath}\{_defaultProjectFileName}") == 1
                ), _projectsListFilePath));
        }


        [TestMethod]
        public void CreateProject_ShouldReturnBadOpResultIfProjectFolderDoesNotExist()
        {
            // arrange
            string badPath = "not an existing folder path";
            var model = new Project
            {
                Name = "project Name",
                Path = badPath
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(false);
            _directoryInfoFactoryMock.Setup(m => m.Create(badPath)).Returns(directoryInfoMock.Object);

            // act
            var opResult = _repository.CreateProject(model);

            // assert
            Assert.IsFalse(opResult.Success);
            Assert.AreEqual(ErrorCode.FOLDER_DOES_NOT_EXIST, opResult.ErrorCode);
        }

        [TestMethod]
        public void CreateProject_ShouldInitializeProject_DumpTheProjectFile()
        {
            // arrange 
            var newProjectPath = @"C:\any\new\path";
            var model = new Project
            {
                Name = "project Name",
                Path = newProjectPath
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(true);
            _directoryInfoFactoryMock
                .Setup(m => m.Create(newProjectPath))
                .Returns(directoryInfoMock.Object);

            // act
            var opResult = _repository.CreateProject(model);

            // assert
            _projectFileConverterMock
                .Verify(m => m.Dump(
                    It.Is<Project>(p => p.Name == "project Name" && p.Path == newProjectPath)
                    , $@"{newProjectPath}\{_defaultProjectFileName}"));
        }

        [TestMethod]
        public void SaveProject_ShouldDumpTheAnnexDefaultRequestFile()
        {
            // arrange 
            var newProjectPath = @"C:\any\new\path";
            var model = new Project
            {
                Name = "project Name",
                Path = newProjectPath
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(true);
            _directoryInfoFactoryMock
                .Setup(m => m.Create(newProjectPath))
                .Returns(directoryInfoMock.Object);

            // act
            var opResult = _repository.CreateProject(model);

            // assert
            _annexFileConverterMock
                .Verify(m => m.Dump(
                    It.Is<Node>(r =>
                        r.Name == "Request 1"),
                    It.Is<string>(s =>
                        s.StartsWith($@"{newProjectPath}\project Name\")
                        && (new Regex(_guidRegex)).IsMatch(s)
                        && s.EndsWith(".yml")
                    )));
        }

        [TestMethod]
        public void UpdateProject_ShouldUpdateAndDumpProjectFile()
        {
            // arrange
            var updatedProjectPath = "any folder";
            var model = new Project
            {
                Id = projectId,
                Name = "project Name",
                Path = updatedProjectPath,
                Tree = new PackNode
                {
                    Id = Guid.NewGuid(),
                    Name = "Root",
                    Children = new List<Node>
                        {
                            new RequestNode
                            {
                                Id = Guid.NewGuid(),
                                Name = "Request 1",
                                EndpointUrl = "http://test.com",
                                HttpMethod = "POST"
                            },
                            new PackNode
                            {
                                Id = Guid.NewGuid(),
                                Name = "Empty pack",
                                Children = new List<Node>()
                            }
                        }
                }
            };
            _projectFileConverterMock
                .Setup(m => m.Dump(It.IsAny<Project>(), It.IsAny<string>()))
                .Returns(new OpResult
                {
                    Success = true
                });

            // act
            var opResult = _repository.UpdateProject(model);

            // assert

            Assert.IsTrue(opResult.Success);

            _projectConfigsListFileConverterMock.Verify(m => m.Read(_projectsListFilePath));
            _mapperMock.Verify(m => m.Map<Project>(model));
            _projectFileConverterMock.Verify(m => m.Dump(
                It.Is<Project>(p =>
                   p.Name == "project Name"
                   && p.Path == updatedProjectPath),
                $@"{updatedProjectPath}\{_defaultProjectFileName}"));
        }

        [TestMethod]
        public void UpdateProject_ShouldUpdateAndDumpAnnexFiles()
        {
            // arrange
            var updatedProjectPath = "any folder";
            var model = new Project
            {
                Id = projectId,
                Name = "project Name",
                Path = updatedProjectPath,
                Tree = new PackNode
                {
                    Id = Guid.NewGuid(),
                    Name = "Root",
                    Children = new List<Node>
                        {
                            new RequestNode
                            {
                                Id = Guid.NewGuid(),
                                Name = "Request 1",
                                EndpointUrl = "http://test.com",
                                HttpMethod = "POST"
                            },
                            new PackNode
                            {
                                Id = Guid.NewGuid(),
                                Name = "Empty pack",
                                Children = new List<Node>()
                            }
                        }
                }
            };
            _projectFileConverterMock
                .Setup(m => m.Dump(It.IsAny<Project>(), It.IsAny<string>()))
                .Returns(new OpResult
                {
                    Success = true
                });

            // act
            var opResult = _repository.UpdateProject(model);

            // assert
            _annexFileConverterMock
                .Verify(m => m.Dump(
                    It.Is<Node>(r =>
                        r.Name == "Request 1" && ((RequestNode)r).EndpointUrl == "http://test.com" && ((RequestNode)r).HttpMethod == "POST"),
                    It.Is<string>(s =>
                        s.StartsWith($@"{updatedProjectPath}\project Name\")
                        && (new Regex(_guidRegex)).IsMatch(s)
                        && s.EndsWith(".yml")
                    )), Times.Once);
        }

        [TestMethod]
        public void GetProject_ShouldReadAnnexFilesRecursively()
        {
            // arrange
            var projectId = Guid.NewGuid();
            var projectPath = "projectPath";
            var nestedPack1Id = Guid.NewGuid();
            var nestedRequest1Id = Guid.NewGuid();
            var nestedRequest2Id = Guid.NewGuid();
            var nestedRequest3Id = Guid.NewGuid();
            _projectConfigsListFileConverterMock
                .Setup(m => m.Read(It.IsAny<string>()))
                .Returns(new OpResult<ProjectsConfigsList>
                {
                    Success = true,
                    Object = new ProjectsConfigsList
                    {
                        new Project { Id = projectId, Path = projectPath }
                    }
                });
            var fileInfoMock = new Mock<IFileInfo>();
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock
                .Setup(m => m.FullName)
                .Returns(projectPath);
            fileInfoMock
                .Setup(m => m.Directory)
                .Returns(directoryInfoMock.Object);
            _fileInfoFactoryMock
                .Setup(m => m.Create(It.IsAny<string>()))
                .Returns(fileInfoMock.Object);
            _projectFileConverterMock
                .Setup(m => m.Read(projectPath))
                .Returns(new OpResult<Project>
                {
                    Success = true,
                    Object = new Project
                    {
                        Id = projectId,
                        Tree = new PackNode
                        {
                            Children = new List<Node>
                            {
                                new RequestNode
                                {
                                    Id = nestedRequest1Id,
                                },
                                new RequestNode
                                {
                                    Id = nestedRequest2Id,
                                },
                                new PackNode
                                {
                                    Id = nestedPack1Id,
                                    Children = new List<Node>
                                    {
                                        new RequestNode
                                        {
                                            Id = nestedRequest3Id
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            _annexFileConverterMock
                .Setup(m => m.Read<RequestNode>(Path.Combine(projectPath, $"{nestedRequest1Id}.yml")))
                .Returns(new OpResult<RequestNode>
                {
                    Success = true,
                    Object = new RequestNode
                    {
                        Id = nestedRequest1Id,
                        EndpointUrl = "endpoint url 1",
                        HttpMethod = "httpMethod 1"
                    }
                });
            _annexFileConverterMock
                .Setup(m => m.Read<RequestNode>(Path.Combine(projectPath, $"{nestedRequest2Id}.yml")))
                .Returns(new OpResult<RequestNode>
                {
                    Success = true,
                    Object = new RequestNode
                    {
                        Id = nestedRequest2Id,
                        EndpointUrl = "endpoint url 2",
                        HttpMethod = "httpMethod 2"
                    }
                });
            _annexFileConverterMock
                .Setup(m => m.Read<RequestNode>(Path.Combine(projectPath, $"{nestedRequest3Id}.yml")))
                .Returns(new OpResult<RequestNode>
                {
                    Success = true,
                    Object = new RequestNode
                    {
                        Id = nestedRequest3Id,
                        EndpointUrl = "endpoint url 3",
                        HttpMethod = "httpMethod 3"
                    }
                });

            // act
            var project = _repository.GetProject(projectId);

            // assert read is called on project file
            _projectFileConverterMock
                .Verify(m => m.Read(projectPath), Times.Once);

            // assert read methods are called for annex files
            _annexFileConverterMock.Verify(m => m.Read<RequestNode>(Path.Combine(projectPath, $"{nestedRequest1Id}.yml")), Times.Once);
            _annexFileConverterMock.Verify(m => m.Read<RequestNode>(Path.Combine(projectPath, $"{nestedRequest2Id}.yml")), Times.Once);
            _annexFileConverterMock.Verify(m => m.Read<RequestNode>(Path.Combine(projectPath, $"{nestedRequest3Id}.yml")), Times.Once);

            // assert project object is populated with annex file loads
            var rootPack = project.Object.Tree as PackNode;
            Assert.IsNotNull(rootPack.Children[0] as RequestNode);
            var req1 = rootPack.Children[0] as RequestNode;
            Assert.AreEqual(nestedRequest1Id, req1.Id);
            Assert.AreEqual("endpoint url 1", req1.EndpointUrl);
            Assert.AreEqual("httpMethod 1", req1.HttpMethod);

            Assert.IsNotNull(rootPack.Children[1] as RequestNode);
            var req2 = rootPack.Children[1] as RequestNode;
            Assert.AreEqual(nestedRequest2Id, req2.Id);
            Assert.AreEqual("endpoint url 2", req2.EndpointUrl);
            Assert.AreEqual("httpMethod 2", req2.HttpMethod);

            Assert.IsNotNull(rootPack.Children[2] as PackNode);
            var pack1 = rootPack.Children[2] as PackNode;
            Assert.AreEqual(nestedPack1Id, pack1.Id);
            Assert.IsNotNull(pack1.Children[0] as RequestNode);
            var req3 = pack1.Children[0] as RequestNode;
            Assert.AreEqual(nestedRequest3Id, req3.Id);
            Assert.AreEqual("endpoint url 3", req3.EndpointUrl);
            Assert.AreEqual("httpMethod 3", req3.HttpMethod);
        }
    }
}
