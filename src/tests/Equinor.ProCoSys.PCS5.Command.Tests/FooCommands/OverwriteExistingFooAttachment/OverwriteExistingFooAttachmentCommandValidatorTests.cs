using System;
using System.IO;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.OverwriteExistingFooAttachment;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.OverwriteExistingFooAttachment;

[TestClass]
public class OverwriteExistingFooAttachmentCommandValidatorTests
{
    private OverwriteExistingFooAttachmentCommandValidator _dut;
    private Mock<IFooValidator> _fooValidatorMock;
    private Mock<IProjectValidator> _projectValidatorMock;
    private Mock<IAttachmentService> _attachmentServiceMock;
    private OverwriteExistingFooAttachmentCommand _command;
    private readonly string _fileName = "a.txt";

    [TestInitialize]
    public void Setup_OkState()
    {
        _command = new OverwriteExistingFooAttachmentCommand(Guid.NewGuid(), _fileName, "r", new MemoryStream());
        _projectValidatorMock = new Mock<IProjectValidator>();
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(x => x.FooExistsAsync(_command.FooGuid, default))
            .ReturnsAsync(true);
        _attachmentServiceMock = new Mock<IAttachmentService>();
        _attachmentServiceMock.Setup(x => x.AttachmentWithFilenameExistsForSourceAsync(
                _command.FooGuid, 
                _command.FileName))
            .ReturnsAsync(true);
        _dut = new OverwriteExistingFooAttachmentCommandValidator(
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
    public async Task Validate_ShouldFail_When_AttachmentWithFilenameNotExists()
    {
        // Arrange
        _attachmentServiceMock.Setup(x => x.AttachmentWithFilenameExistsForSourceAsync(
                _command.FooGuid,
                _command.FileName))
            .ReturnsAsync(false);

        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Foo don't have an attachment with filename {_command.FileName}!"));
    }
}
