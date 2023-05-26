using System;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFoo;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.UpdateFoo;

[TestClass]
public class UpdateFooCommandHandlerTests : TestsBase
{
    private readonly string _newTitle = "newTitle";
    private readonly string _existingTitle = "existingTitle";
    private readonly string _newText = "newText";
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private Mock<IFooRepository> _fooRepositoryMock;
    private Foo _existingFoo;

    private UpdateFooCommand _command;
    private UpdateFooCommandHandler _dut;

    [TestInitialize]
    public void Setup()
    {
        var project = new Project(TestPlantA, Guid.NewGuid(), "P", "D");
        _existingFoo = new Foo(TestPlantA, project, _existingTitle);
        _fooRepositoryMock = new Mock<IFooRepository>();
        _fooRepositoryMock.Setup(r => r.TryGetByGuidAsync(_existingFoo.Guid))
            .ReturnsAsync(_existingFoo);

        _command = new UpdateFooCommand(_existingFoo.Guid, _newTitle, _newText, _rowVersion);

        _dut = new UpdateFooCommandHandler(
            _fooRepositoryMock.Object,
            _unitOfWorkMock.Object,
            new Mock<ILogger<UpdateFooCommandHandler>>().Object);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldUpdateFoo()
    {
        // Arrange
        Assert.AreEqual(_existingTitle, _existingFoo.Title);

        // Act
        await _dut.Handle(_command, default);

        // Assert
        Assert.AreEqual(_newTitle, _existingFoo.Title);
        Assert.AreEqual(_newText, _existingFoo.Text);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldSave()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _unitOfWorkMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldSetAndReturnRowVersion()
    {
        // Act
        var result = await _dut.Handle(_command, default);

        // Assert
        // In real life EF Core will create a new RowVersion when save.
        // Since UnitOfWorkMock is a Mock this will not happen here, so we assert that RowVersion is set from command
        Assert.AreEqual(_rowVersion, result.Data);
        Assert.AreEqual(_rowVersion, _existingFoo.RowVersion.ConvertToString());
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldAddFooEditedEvent()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        Assert.IsInstanceOfType(_existingFoo.DomainEvents.Last(), typeof(FooUpdatedEvent));
    }
}
