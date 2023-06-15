using System;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using Equinor.ProCoSys.PCS5.Test.Common;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.CreateFoo;

[TestClass]
public class CreateFooCommandHandlerTests : TestsBase
{
    private Mock<IFooRepository> _fooRepositoryMock;
    private Mock<IProjectRepository> _projectRepositoryMock;

    private readonly string _projectName = "Project";
    private readonly int _projectIdOnExisting = 10;

    private Foo _fooAddedToRepository;

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
        var project = new Project(TestPlantA, Guid.NewGuid(), _projectName, "");
        project.SetProtectedIdForTesting(_projectIdOnExisting);
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _projectRepositoryMock
            .Setup(x => x.TryGetProjectByNameAsync(_projectName))
            .ReturnsAsync(project);

        _command = new CreateFooCommand("Foo", _projectName);

        _dut = new CreateFooCommandHandler(
            _plantProviderMock.Object,
            _fooRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _projectRepositoryMock.Object,
            new Mock<ILogger<CreateFooCommandHandler>>().Object);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldReturn_GuidAndRowVersion()
    {
        // Act
        var result = await _dut.Handle(_command, default);

        // Assert
        Assert.IsInstanceOfType(result.Data, typeof(GuidAndRowVersion));
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldAddFooToRepository()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        Assert.IsNotNull(_fooAddedToRepository);
        Assert.AreEqual(_command.Title, _fooAddedToRepository.Title);
        Assert.AreEqual(_projectIdOnExisting, _fooAddedToRepository.ProjectId);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldSave()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldThrewException_WhenProjectNotExists()
    {
        // Arrange
        _projectRepositoryMock
            .Setup(x => x.TryGetProjectByNameAsync(_projectName))
            .ReturnsAsync((Project)null);

        // Act and Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => _dut.Handle(_command, default));
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldAddFooCreatedEvent()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        Assert.IsInstanceOfType(_fooAddedToRepository.DomainEvents.First(), typeof(FooCreatedEvent));
    }
}
