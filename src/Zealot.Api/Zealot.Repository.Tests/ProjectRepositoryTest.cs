using System.IO;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemWrap;
using Zealot.Domain;
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
        private ProjectRepository _repository;


        [TestInitialize]
        public void TestInitialize()
        {
            _directoryInfoFactoryMock = new Mock<IDirectoryInfoFactory>();
            _projectFileConverterMock = new Mock<IJsonFileConverter<Project>>();
            _projectConfigsListFileConverterMock = new Mock<IJsonFileConverter<ProjectsConfigsList>>();
            _mapperMock = new Mock<IMapper>();
            _repository = new ProjectRepository(
                _directoryInfoFactoryMock.Object
                , _projectFileConverterMock.Object
                , _projectConfigsListFileConverterMock.Object
                , _mapperMock.Object
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
    }
}
