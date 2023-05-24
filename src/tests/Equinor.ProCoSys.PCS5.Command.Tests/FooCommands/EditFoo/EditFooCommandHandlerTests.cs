using System;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Command.FooCommands.EditFoo;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.EditFoo;

[TestClass]
public class EditFooCommandHandlerTests : CommandHandlerTestsBase
{
    private readonly string _newTitle = "newTitle";
    private readonly string _existingTitle = "existingTitle";
    private readonly string _newText = "newText";
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private Mock<IFooRepository> _fooRepositoryMock;
    private Foo _existingFoo;

    private EditFooCommand _command;
    private EditFooCommandHandler _dut;

    [TestInitialize]
    public void Setup()
    {
        var project = new Project(TestPlant, Guid.NewGuid(), "P", "D");
        _existingFoo = new Foo(TestPlant, project, _existingTitle);
        _fooRepositoryMock = new Mock<IFooRepository>();
        _fooRepositoryMock.Setup(r => r.GetByGuidAsync(_existingFoo.Guid))
            .ReturnsAsync(_existingFoo);

        _command = new EditFooCommand(_existingFoo.Guid, _newTitle, _newText, _rowVersion);

        _dut = new EditFooCommandHandler(
            _fooRepositoryMock.Object,
            _unitOfWorkMock.Object,
            new Mock<ILogger<EditFooCommandHandler>>().Object);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldUpdateFoo()
    {
        Assert.AreEqual(_existingTitle, _existingFoo.Title);

        await _dut.Handle(_command, default);

        Assert.AreEqual(_newTitle, _existingFoo.Title);
        Assert.AreEqual(_newText, _existingFoo.Text);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldSave()
    {
        await _dut.Handle(_command, default);

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
        var result = await _dut.Handle(_command, default);

        Assert.IsInstanceOfType(_existingFoo.DomainEvents.Last(), typeof(FooUpdatedEvent));
    }
}
