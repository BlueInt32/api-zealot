using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SystemWrap;
using Zealot.Domain.Models;
using Zealot.Domain.Objects;
using Zealot.Repository.IO;

namespace Zealot.Repository.Tests
{
    [TestClass]
    public class ProjectRepositoryTest
    {
        private Mock<IDirectoryInfoFactory> _directoryInfoFactoryMock;
        private Mock<IObjectJsonDump<Project>> _projectDumpMock;
        private ProjectRepository _repository;


        [TestInitialize]
        public void TestInitialize()
        {
            _directoryInfoFactoryMock = new Mock<IDirectoryInfoFactory>();
            _projectDumpMock = new Mock<IObjectJsonDump<Project>>();
            _repository = new ProjectRepository(
                _directoryInfoFactoryMock.Object
                , _projectDumpMock.Object);
        }
        [TestMethod]
        public void SaveProject_ShouldCheckFolderExists()
        {
            // arrange
            var model = new ProjectModel
            {
                Name = "project Name",
                Folder = "not an existing folder path"
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(false);
            _directoryInfoFactoryMock
                .Setup(m => m.Create("not an existing folder path"))
                .Returns(directoryInfoMock.Object);

            // act
            var opResult = _repository.SaveProject(model);

            // assert
            Assert.IsFalse(opResult.Success);
            Assert.AreEqual(Zealot.Domain.Constants.FOLDER_DOES_NOT_EXISTS, opResult.ErrorCode);
        }

        [TestMethod]
        public void SaveProject_ShouldInitializeProjectAndDumpIt()
        {
            // arrange 
            var model = new ProjectModel
            {
                Name = "project Name",
                Folder = "any folder"
            };
            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.Setup(m => m.Exists).Returns(true);
            _directoryInfoFactoryMock
                .Setup(m => m.Create("any folder"))
                .Returns(directoryInfoMock.Object);

            // act
            var opResult = _repository.SaveProject(model);

            // assert
            _projectDumpMock
                .Verify(m => m.Dump(It.Is<Project>(
                    p => p.Name == "project Name"
                    ), It.Is<string>(p => p == Path.Combine("any folder", "project.json"))));


        }
    }
}
