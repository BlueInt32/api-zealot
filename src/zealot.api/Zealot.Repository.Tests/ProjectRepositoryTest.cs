using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemWrap;
using Zealot.Domain;
using Zealot.Domain.Enums;
using Zealot.Domain.Models;
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
        private Mock<IRequestFileConverter> _annexFileConverterMock;
        private ProjectRepository _repository;

        private Guid projectId = Guid.NewGuid();
        private string projectPath = "any folder";


        [TestInitialize]
        public void TestInitialize()
        {
            _directoryInfoFactoryMock = new Mock<IDirectoryInfoFactory>();
            _fileInfoFactoryMock = new Mock<IFileInfoFactory>();
            _projectFileConverterMock = new Mock<IJsonFileConverter<Project>>();
            _projectConfigsListFileConverterMock = new Mock<IJsonFileConverter<ProjectsConfigsList>>();
            _mapperMock = new Mock<IMapper>();
            _annexFileConverterMock = new Mock<IRequestFileConverter>();
            _repository = new ProjectRepository(
                _directoryInfoFactoryMock.Object
                , _fileInfoFactoryMock.Object
                , _projectFileConverterMock.Object
                , _projectConfigsListFileConverterMock.Object
                , _mapperMock.Object
                , _annexFileConverterMock.Object
                );

            _projectConfigsListFileConverterMock
                .Setup(m => m.Read(@"D:\_Prog\Projects\Zealot\test\projects.json"))
                .Returns(new OpResult<ProjectsConfigsList>
                {
                    Object = new ProjectsConfigsList
                    {
                        new ProjectConfig
                        {
                            Id = projectId,
                            Path = projectPath

                        }
                    }
                });
        }
        [TestMethod]
        public void SaveProject_ShouldCheckFolderExists()
        {
            // arrange
            var model = new ProjectModel
            {
                Name = "project Name",
                Path = "not an existing folder path"
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(false);
            _directoryInfoFactoryMock
                .Setup(m => m.Create("not an existing folder path"))
                .Returns(directoryInfoMock.Object);

            // act
            var opResult = _repository.CreateProject(model);

            // assert
            Assert.IsFalse(opResult.Success);
            Assert.AreEqual(ErrorCode.FOLDER_DOES_NOT_EXIST, opResult.ErrorCode);
        }

        [TestMethod]
        public void SaveProject_ShouldInitializeProjectAndDumpIt()
        {
            // arrange 
            var model = new ProjectModel
            {
                Name = "project Name",
                Path = "any new folder"
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(true);
            _directoryInfoFactoryMock
                .Setup(m => m.Create("any new folder"))
                .Returns(directoryInfoMock.Object);

            // act
            var opResult = _repository.CreateProject(model);

            // assert
            _projectConfigsListFileConverterMock
                .Verify(m => m.Dump(It.Is<ProjectsConfigsList>(
                    l => l.Count == 2
                    ), @"D:\_Prog\Projects\Zealot\test\projects.json"));
        }

        [TestMethod]
        public void UpdateProject_ShouldWorkWithEmptyProject()
        {
            // arrange
            var model = new ProjectModel
            {
                Id = projectId,
                Name = "project Name",
                Path = "any folder",
                Tree = new SubTreeModel
                {
                    Type = TreeNodeType.Pack
                }
            };
            _mapperMock
                .Setup(m => m.Map<Project>(It.IsAny<ProjectModel>()))
                .Returns(new Project
                {
                    Id = projectId,
                    Tree = new PackNode
                    {
                        Id = Guid.NewGuid(),
                        Name = "Root",
                        Children = new List<INode>
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
                                Name = "Empty pack"
                            }

                        }


                    }
                });
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

            _projectConfigsListFileConverterMock.Verify(m => m.Read(@"D:\_Prog\Projects\Zealot\test\projects.json"));
            _mapperMock.Verify(m => m.Map<Project>(model));
            _projectFileConverterMock.Verify(m => m.Dump(It.IsAny<Project>(), "any folder"));

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
                        new ProjectConfig { Id = projectId, Path = projectPath }
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
                            Children = new List<INode>
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
                                    Children = new List<INode>
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
                .Setup(m => m.Read(Path.Combine(projectPath, $"{nestedRequest1Id}.yml")))
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
                .Setup(m => m.Read(Path.Combine(projectPath, $"{nestedRequest2Id}.yml")))
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
                .Setup(m => m.Read(Path.Combine(projectPath, $"{nestedRequest3Id}.yml")))
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
            _annexFileConverterMock.Verify(m => m.Read(Path.Combine(projectPath, $"{nestedRequest1Id}.yml")), Times.Once);
            _annexFileConverterMock.Verify(m => m.Read(Path.Combine(projectPath, $"{nestedRequest2Id}.yml")), Times.Once);
            _annexFileConverterMock.Verify(m => m.Read(Path.Combine(projectPath, $"{nestedRequest3Id}.yml")), Times.Once);

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

            Assert.IsNotNull(rootPack.Children[2] as RequestNode);
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
