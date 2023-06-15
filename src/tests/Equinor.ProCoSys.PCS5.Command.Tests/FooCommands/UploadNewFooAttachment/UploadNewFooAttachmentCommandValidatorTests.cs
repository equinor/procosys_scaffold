using System;
using System.IO;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.UploadNewFooAttachment;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.UploadNewFooAttachment;

[TestClass]
public class UploadNewFooAttachmentCommandValidatorTests
{
    private UploadNewFooAttachmentCommandValidator _dut;
    private Mock<IFooValidator> _fooValidatorMock;
    private Mock<IProjectValidator> _projectValidatorMock;
    private Mock<IAttachmentService> _attachmentServiceMock;
    private UploadNewFooAttachmentCommand _command;

    [TestInitialize]
    public void Setup_OkState()
    {
        _command = new UploadNewFooAttachmentCommand(Guid.NewGuid(), "f.txt", new MemoryStream());
        _projectValidatorMock = new Mock<IProjectValidator>();
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(x => x.FooExistsAsync(_command.FooGuid, default))
            .ReturnsAsync(true);
        _attachmentServiceMock = new Mock<IAttachmentService>();
        _dut = new UploadNewFooAttachmentCommandValidator(
            _projectValidatorMock.Object,
            _fooValidatorMock.Object,
            _attachmentServiceMock.Object);
    }

    [TestMethod]
    public async Task Validate_ShouldBeValid_WhenOkState()
    {
        var result = await _dut.ValidateAsync(_command);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public async Task Validate_ShouldFail_When_FooNotExists()
    {
        // Arrange
        _fooValidatorMock.Setup(inv => inv.FooExistsAsync(_command.FooGuid, default))
            .ReturnsAsync(false);

        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Foo with this guid does not exist!"));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_When_FooIsVoided()
    {
        // Arrange
        _fooValidatorMock.Setup(inv => inv.FooIsVoidedAsync(_command.FooGuid, default))
            .ReturnsAsync(true);

        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Foo is voided!"));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_When_ProjectIsClosed()
    {
        // Arrange
        _projectValidatorMock.Setup(x => x.IsClosedForFoo(_command.FooGuid, default))
            .ReturnsAsync(true);

        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Project is closed!"));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_When_AttachmentWithFilenameExists()
    {
        // Arrange
        _attachmentServiceMock.Setup(x => x.FilenameExistsForSourceAsync(
                _command.FooGuid, 
                _command.FileName))
            .ReturnsAsync(true);

        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Foo already has an attachment with filename {_command.FileName}! Please rename file or choose to overwrite"));
    }
}
