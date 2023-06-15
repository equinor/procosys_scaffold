using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Links;
using Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFooLink;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.DeleteFooLink;

[TestClass]
public class DeleteFooLinkCommandHandlerTests : TestsBase
{
    private readonly string _rowVersion = "AAAAAAAAABA=";
    private DeleteFooLinkCommandHandler _dut;
    private DeleteFooLinkCommand _command;
    private Mock<ILinkService> _linkServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _command = new DeleteFooLinkCommand(Guid.NewGuid(), Guid.NewGuid(), _rowVersion);

        _linkServiceMock = new Mock<ILinkService>();


        _dut = new DeleteFooLinkCommandHandler(_linkServiceMock.Object);
    }

    [TestMethod]
    public async Task HandlingCommand_Should_CallDelete_OnLinkService()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _linkServiceMock.Verify(u => u.DeleteAsync(
            _command.LinkGuid,
            _command.RowVersion,
            default), Times.Exactly(1));
    }
}
