using System;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.DeleteFoo;

[TestClass]
public class DeleteFooCommandHandlerTests : TestsBase
{
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private Mock<IFooRepository> _fooRepositoryMock;
    private Foo _existingFoo;

    private DeleteFooCommand _command;
    private DeleteFooCommandHandler _dut;

    [TestInitialize]
    public void Setup()
    {
        var project = new Project(TestPlantA, Guid.NewGuid(), "P", "D");
        _existingFoo = new Foo(TestPlantA, project, "Foo");
        _fooRepositoryMock = new Mock<IFooRepository>();
        _fooRepositoryMock.Setup(r => r.TryGetByGuidAsync(_existingFoo.Guid))
            .ReturnsAsync(_existingFoo);

        _command = new DeleteFooCommand(_existingFoo.Guid, _rowVersion);

        _dut = new DeleteFooCommandHandler(
            _fooRepositoryMock.Object,
            _unitOfWorkMock.Object,
            new Mock<ILogger<DeleteFooCommandHandler>>().Object);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldDeleteFooFromRepository()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _fooRepositoryMock.Verify(r => r.Remove(_existingFoo), Times.Once);
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
    public async Task HandlingCommand_ShouldSetAndReturnRowVersion()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        // In real life EF Core will create a new RowVersion when save.
        // Since UnitOfWorkMock is a Mock this will not happen here, so we assert that RowVersion is set from command
        Assert.AreEqual(_rowVersion, _existingFoo.RowVersion.ConvertToString());
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldAddFooDeletedEvent()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        Assert.IsInstanceOfType(_existingFoo.DomainEvents.Last(), typeof(FooDeletedEvent));
    }
}
