using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.ForeignApi.MainApi.Project;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.CreateFoo
{
    [TestClass]
    public class CreateFooCommandHandlerTests : CommandHandlerTestsBase
    {
        private Mock<IFooRepository> _fooRepositoryMock;
        private Mock<IProjectApiService> _projectApiServiceMock;
        private Mock<IProjectRepository> _projectRepositoryMock;

        private readonly string _projectName = "Project";
        private readonly string _projectDescription = "Project Desc";
        private readonly int _projectIdOnNew = 1;

        private Foo _fooAddedToRepository;
        private Project _projectAddedToRepository;
        private ProCoSysProject _proCoSysProject;

        private CreateFooCommandHandler _dut;
        private CreateFooCommand _command;

        [TestInitialize]
        public void Setup()
        {
            _fooRepositoryMock = new Mock<IFooRepository>();
            _fooRepositoryMock
                .Setup(x => x.Add(It.IsAny<Foo>()))
                .Callback<Foo>(foo =>
                {
                    _fooAddedToRepository = foo;
                });
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _projectRepositoryMock
                .Setup(x => x.Add(It.IsAny<Project>()))
                .Callback<Project>(project =>
                {
                    _projectAddedToRepository = project;
                    project.SetProtectedIdForTesting(_projectIdOnNew);
                });
            _proCoSysProject = new ProCoSysProject {Name = _projectName, Description = _projectDescription };
            _projectApiServiceMock = new Mock<IProjectApiService>();
            _projectApiServiceMock
                .Setup(x => x.TryGetProjectAsync(TestPlant, _projectName))
                .ReturnsAsync(_proCoSysProject);

            _command = new CreateFooCommand("Foo", _projectName);

            _dut = new CreateFooCommandHandler(
                PlantProviderMock.Object,
                _fooRepositoryMock.Object,
                UnitOfWorkMock.Object,
                _projectRepositoryMock.Object,
                _projectApiServiceMock.Object,
                new Mock<ILogger<CreateFooCommandHandler>>().Object);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldReturn_IdAndRowVersion()
        {
            // Act
            var result = await _dut.Handle(_command, default);

            // Assert
            Assert.IsInstanceOfType(result.Data, typeof(IdAndRowVersion));
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldAddFooToRepository_WhenProjectNotExists()
        {
            // Act
            await _dut.Handle(_command, default);

            // Assert
            Assert.IsNotNull(_fooAddedToRepository);
            Assert.AreEqual(_projectIdOnNew, _fooAddedToRepository.ProjectId);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldAddFooToRepository_WhenProjectExists()
        {
            // Arrange
            var project = new Project(TestPlant, Guid.NewGuid(), _projectName, "");
            var projectIdOnExisting = 10;
            project.SetProtectedIdForTesting(projectIdOnExisting);
            _projectRepositoryMock.Setup(r => r.GetProjectOnlyByNameAsync(_projectName)).ReturnsAsync(project);
            // Act
            await _dut.Handle(_command, default);

            // Assert
            Assert.IsNotNull(_fooAddedToRepository);
            Assert.AreEqual(projectIdOnExisting, _fooAddedToRepository.ProjectId);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldAddProjectToRepository_WhenProjectNotExists()
        {
            // Act
            await _dut.Handle(_command, default);

            // Assert
            Assert.IsNotNull(_projectAddedToRepository);
            Assert.AreEqual(_projectIdOnNew, _projectAddedToRepository.Id);
            Assert.AreEqual(_projectName, _projectAddedToRepository.Name);
            Assert.AreEqual(_projectDescription, _projectAddedToRepository.Description);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldNotAddAnyProjectToRepository_WhenProjectAlreadyExists()
        {
            // Arrange
            var project = new Project(TestPlant, Guid.NewGuid(), _projectName, "");
            _projectRepositoryMock.Setup(r => r.GetProjectOnlyByNameAsync(_projectName)).ReturnsAsync(project);

            // Act
            await _dut.Handle(_command, default);

            // Assert
            Assert.IsNull(_projectAddedToRepository);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldSaveOnce_WhenProjectAlreadyExists()
        {
            // Arrange
            var project = new Project(TestPlant, Guid.NewGuid(), _projectName, "");
            _projectRepositoryMock.Setup(r => r.GetProjectOnlyByNameAsync(_projectName)).ReturnsAsync(project);

            // Act
            await _dut.Handle(_command, default);

            // Assert
            UnitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldSaveTwice_WhenProjectNotExists()
        {
            // Act
            await _dut.Handle(_command, default);

            // Assert
            UnitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Exactly(2));
        }
    }
}
