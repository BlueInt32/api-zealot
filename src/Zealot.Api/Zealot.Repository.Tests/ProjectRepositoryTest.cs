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
                Path = "any folder"
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(true);
            _directoryInfoFactoryMock
                .Setup(m => m.Create("any folder"))
                .Returns(directoryInfoMock.Object);
            _projectConfigsListFileConverterMock
                .Setup(m => m.Read(@"D:\_Prog\Projects\Zealot\test\projects.json"))
                .Returns(new OpResult<ProjectsConfigsList>
                {
                    Object = new ProjectsConfigsList{
                    new ProjectConfig{ }

                }
                });

            // act
            var opResult = _repository.CreateProject(model);

            // assert
            _projectFileConverterMock
                .Verify(m => m.Dump(It.Is<Project>(
                    p => p.Name == "project Name"
                    ), It.Is<string>(p => p == Path.Combine("any folder", "project.json"))));


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
                        Name = "projectName",
                        Tree = new Node
                        {
                            Type = TreeNodeType.Pack,
                            Children = new List<Node>
                            {
                                new Node
                                {
                                    Id = nestedRequest1Id,
                                    Type = TreeNodeType.Request
                                },
                                new Node
                                {
                                    Id = nestedRequest2Id,
                                    Type = TreeNodeType.Request
                                },
                                new Node
                                {
                                    Id = nestedPack1Id,
                                    Type = TreeNodeType.Pack,
                                    Children = new List<Node>
                                    {
                                        new Node
                                        {
                                            Id = nestedRequest3Id,
                                            Type = TreeNodeType.Request
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            _annexFileConverterMock
                .Setup(m => m.Read(Path.Combine(projectPath, $"{nestedRequest1Id}.yml")))
                .Returns(new OpResult<Request>
                {
                    Success = true,
                    Object = new Request
                    {
                        Id = nestedRequest1Id,
                        EndpointUrl = "endpoint url 1",
                        HttpMethod = "httpMethod 1"
                    }
                });
            _annexFileConverterMock
                .Setup(m => m.Read(Path.Combine(projectPath, $"{nestedRequest2Id}.yml")))
                .Returns(new OpResult<Request>
                {
                    Success = true,
                    Object = new Request
                    {
                        Id = nestedRequest2Id,
                        EndpointUrl = "endpoint url 2",
                        HttpMethod = "httpMethod 2"
                    }
                });
            _annexFileConverterMock
                .Setup(m => m.Read(Path.Combine(projectPath, $"{nestedRequest3Id}.yml")))
                .Returns(new OpResult<Request>
                {
                    Success = true,
                    Object = new Request
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
            Assert.IsNotNull(project.Object.Tree.Children[0] as Request);
            var req1 = project.Object.Tree.Children[0] as Request;
            Assert.AreEqual(nestedRequest1Id, req1.Id);
            Assert.AreEqual("endpoint url 1", req1.EndpointUrl);
            Assert.AreEqual("httpMethod 1", req1.HttpMethod);

            Assert.IsNotNull(project.Object.Tree.Children[1] as Request);
            var req2 = project.Object.Tree.Children[1] as Request;
            Assert.AreEqual(nestedRequest2Id, req2.Id);
            Assert.AreEqual("endpoint url 2", req2.EndpointUrl);
            Assert.AreEqual("httpMethod 2", req2.HttpMethod);

            Assert.IsNotNull(project.Object.Tree.Children[2] as Node);
            var pack1 = project.Object.Tree.Children[2] as Node;
            Assert.AreEqual(nestedPack1Id, pack1.Id);
            Assert.IsNotNull(pack1.Children[0] as Request);
            var req3 = pack1.Children[0] as Request;
            Assert.AreEqual(nestedRequest3Id, req3.Id);
            Assert.AreEqual("endpoint url 3", req3.EndpointUrl);
            Assert.AreEqual("httpMethod 3", req3.HttpMethod);
        }
    }
}
