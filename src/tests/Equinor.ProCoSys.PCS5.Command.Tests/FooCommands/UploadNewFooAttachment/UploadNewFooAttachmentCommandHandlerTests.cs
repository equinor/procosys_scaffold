using System;
using System.IO;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.UploadNewFooAttachment;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.UploadNewFooAttachment;

[TestClass]
public class UploadNewFooAttachmentCommandHandlerTests : TestsBase
{
    private readonly string _rowVersion = "AAAAAAAAABA=";
    private readonly Guid _guid = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private UploadNewFooAttachmentCommandHandler _dut;
    private UploadNewFooAttachmentCommand _command;
    private Mock<IAttachmentService> _attachmentServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _command = new UploadNewFooAttachmentCommand(Guid.NewGuid(), "T", new MemoryStream());

        _attachmentServiceMock = new Mock<IAttachmentService>();
        _attachmentServiceMock.Setup(a => a.UploadNewAsync(
            nameof(Foo),
            _command.FooGuid,
            _command.FileName,
            _command.Content,
            default)).ReturnsAsync(new AttachmentDto(_guid, _rowVersion));

        _dut = new UploadNewFooAttachmentCommandHandler(_attachmentServiceMock.Object);
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
    public async Task HandlingCommand_Should_CallAdd_OnAttachmentService()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _attachmentServiceMock.Verify(u => u.UploadNewAsync(
            nameof(Foo), 
            _command.FooGuid, 
            _command.FileName,
            _command.Content,
            default), Times.Exactly(1));
    }
}
