using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Links;
using Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFooLink;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.UpdateFooLink;

[TestClass]
public class UpdateFooLinkCommandValidatorTests
{
    private UpdateFooLinkCommandValidator _dut;
    private Mock<IFooValidator> _fooValidatorMock;
    private Mock<IProjectValidator> _projectValidatorMock;
    private Mock<ILinkService> _linkServiceMock;
    private UpdateFooLinkCommand _command;

    [TestInitialize]
    public void Setup_OkState()
    {
        _command = new UpdateFooLinkCommand(Guid.NewGuid(), Guid.NewGuid(), "New title", "New text", "r");
        _projectValidatorMock = new Mock<IProjectValidator>();
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(x => x.FooExistsAsync(_command.FooGuid, default))
            .ReturnsAsync(true);
        _linkServiceMock = new Mock<ILinkService>();
        _linkServiceMock.Setup(x => x.ExistsAsync(_command.LinkGuid))
            .ReturnsAsync(true);

        _dut = new UpdateFooLinkCommandValidator(
            _projectValidatorMock.Object,
            _fooValidatorMock.Object,
            _linkServiceMock.Object);
    }

    [TestMethod]
    public async Task Validate_ShouldBeValid_WhenOkState()
    {
        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
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
    public async Task Validate_ShouldFail_When_LinkNotExists()
    {
        // Arrange
        _linkServiceMock.Setup(x => x.ExistsAsync(_command.LinkGuid))
            .ReturnsAsync(false);

        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Link with this guid does not exist!"));
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
}
