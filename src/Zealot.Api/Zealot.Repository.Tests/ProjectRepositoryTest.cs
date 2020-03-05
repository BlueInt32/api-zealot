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
        private Mock<IJsonFileConverter<Project>> _projectFileConverterMock;
        private Mock<IJsonFileConverter<ProjectsConfigsList>> _projectConfigsListFileConverterMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAnnexFileConverter> _annexFileConverterMock;
        private ProjectRepository _repository;


        [TestInitialize]
        public void TestInitialize()
        {
            _directoryInfoFactoryMock = new Mock<IDirectoryInfoFactory>();
            _projectFileConverterMock = new Mock<IJsonFileConverter<Project>>();
            _projectConfigsListFileConverterMock = new Mock<IJsonFileConverter<ProjectsConfigsList>>();
            _mapperMock = new Mock<IMapper>();
            _annexFileConverterMock = new Mock<IAnnexFileConverter>();
            _repository = new ProjectRepository(
                _directoryInfoFactoryMock.Object
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
            var nestedRequest1Id = Guid.NewGuid();
            var nestedRequest2Id = Guid.NewGuid();
            _projectConfigsListFileConverterMock
                .Setup(m => m.Read(It.IsAny<string>()))
                .Returns(new OpResult<ProjectsConfigsList>
                {
                    Success = true,
                    Object = new ProjectsConfigsList
                    {
                        new ProjectConfig { Id = projectId, Path = "path" }
                    }
                });
            _projectFileConverterMock
                .Setup(m => m.Read("path"))
                .Returns(new OpResult<Project>
                {
                    Success = true,
                    Object = new Project
                    {
                        Id = projectId,
                        Name = "projectName",
                        Tree = new SubTree
                        {
                            Type = TreeNodeType.Pack,
                            Children = new List<SubTree>
                            {
                                new Request
                                {
                                    Id = nestedRequest1Id,
                                    EndpointUrl = "endpoint url 1",
                                    HttpMethod = "httpMedthod 1"
                                },
                                new Request
                                {
                                    Id = nestedRequest2Id,
                                    EndpointUrl = "endpoint url 2",
                                    HttpMethod = "httpMedthod 2"
                                },

                            }
                        }
                    }
                });

            // act
            var project = _repository.GetProject(projectId);

            // assert
            _annexFileConverterMock.Verify(m => m.Read($"{projectPath}{nestedRequest1Id}.yml"), Times.Once);
            _annexFileConverterMock.Verify(m => m.Read($"{projectPath}{nestedRequest2Id}.yml"), Times.Once);

        }
    }
}
