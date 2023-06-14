using System;
using System.IO;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.OverwriteExistingFooAttachment;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.OverwriteExistingFooAttachment;

[TestClass]
public class OverwriteExistingFooAttachmentCommandHandlerTests : TestsBase
{
    private readonly string _newRowVersion = "AAAAAAAAACC=";
    private OverwriteExistingFooAttachmentCommandHandler _dut;
    private OverwriteExistingFooAttachmentCommand _command;
    private Mock<IAttachmentService> _attachmentServiceMock;

    [TestInitialize]
    public void Setup()
    {
        var oldRowVersion = "AAAAAAAAABA=";
        _command = new OverwriteExistingFooAttachmentCommand(Guid.NewGuid(), "T", oldRowVersion, new MemoryStream());

        _attachmentServiceMock = new Mock<IAttachmentService>();
        _attachmentServiceMock.Setup(a => a.UploadOverwriteAsync(
            nameof(Foo),
            _command.FooGuid,
            _command.FileName,
            _command.Content,
            _command.RowVersion,
            default)).ReturnsAsync(_newRowVersion);

        _dut = new OverwriteExistingFooAttachmentCommandHandler(_attachmentServiceMock.Object);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldReturn_NewGuid()
    {
        // Act
        var result = await _dut.Handle(_command, default);

        // Assert
        Assert.AreEqual(_newRowVersion, result.Data);
    }

    [TestMethod]
    public async Task HandlingCommand_Should_CallAdd_OnAttachmentService()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _attachmentServiceMock.Verify(u => u.UploadOverwriteAsync(
            nameof(Foo), 
            _command.FooGuid, 
            _command.FileName,
            _command.Content,
            _command.RowVersion,
            default), Times.Exactly(1));
    }
}
