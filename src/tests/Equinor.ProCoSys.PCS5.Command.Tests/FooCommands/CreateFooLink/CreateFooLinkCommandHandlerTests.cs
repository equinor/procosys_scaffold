using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Application.Dtos;
using Equinor.ProCoSys.PCS5.Application.Interfaces;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooLink;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.CreateFooLink;

[TestClass]
public class CreateFooLinkCommandHandlerTests : TestsBase
{
    private readonly string _rowVersion = "AAAAAAAAABA=";
    private readonly Guid _guid = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private CreateFooLinkCommandHandler _dut;
    private CreateFooLinkCommand _command;
    private Mock<ILinkService> _linkServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _command = new CreateFooLinkCommand(Guid.NewGuid(), "T", "U");

        _linkServiceMock = new Mock<ILinkService>();
        _linkServiceMock.Setup(l => l.AddAsync(
            nameof(Foo),
            _command.FooGuid,
            _command.Title,
            _command.Url, 
            default)).ReturnsAsync(new LinkDto(_command.FooGuid, _guid, _command.Title, _command.Url, _rowVersion));

        _dut = new CreateFooLinkCommandHandler(_linkServiceMock.Object);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldReturn_GuidAndRowVersion()
    {
        // Act
        var result = await _dut.Handle(_command, default);

        // Assert
        Assert.IsInstanceOfType(result.Data, typeof(GuidAndRowVersion));
        Assert.AreEqual(_rowVersion, result.Data.RowVersion);
        Assert.AreEqual(_guid, result.Data.Guid);
    }

    [TestMethod]
    public async Task HandlingCommand_Should_CallAdd_OnLinkService()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _linkServiceMock.Verify(u => u.AddAsync(
            nameof(Foo), 
            _command.FooGuid, 
            _command.Title,
            _command.Url,
            default), Times.Exactly(1));
    }
}
