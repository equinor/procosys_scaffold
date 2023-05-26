using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Application.Interfaces;
using Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFooLink;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.UpdateFooLink;

[TestClass]
public class UpdateFooLinkCommandHandlerTests : TestsBase
{
    private readonly string _rowVersion = "AAAAAAAAABA=";
    private UpdateFooLinkCommandHandler _dut;
    private UpdateFooLinkCommand _command;
    private Mock<ILinkService> _linkServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _command = new UpdateFooLinkCommand(Guid.NewGuid(), Guid.NewGuid(), "T", "U", "R");

        _linkServiceMock = new Mock<ILinkService>();
        _linkServiceMock.Setup(l => l.UpdateAsync(
            _command.LinkGuid,
            _command.Title,
            _command.Url,
            _command.RowVersion,
            default)).ReturnsAsync(_rowVersion);

        _dut = new UpdateFooLinkCommandHandler(_linkServiceMock.Object);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldReturn_RowVersion()
    {
        // Act
        var result = await _dut.Handle(_command, default);

        // Assert
        Assert.IsInstanceOfType(result.Data, typeof(string));
        Assert.AreEqual(_rowVersion, result.Data);
    }

    [TestMethod]
    public async Task HandlingCommand_Should_CallUpdate_OnLinkService()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _linkServiceMock.Verify(u => u.UpdateAsync(
            _command.LinkGuid,
            _command.Title,
            _command.Url,
            _command.RowVersion,
            default), Times.Exactly(1));
    }
}
