using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooLink;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.CreateFooLink;

[TestClass]
public class CreateFooLinkCommandValidatorTests
{
    private CreateFooLinkCommandValidator _dut;
    private Mock<IFooValidator> _fooValidatorMock;
    private Mock<IProjectValidator> _projectValidatorMock;
    private CreateFooLinkCommand _command;

    [TestInitialize]
    public void Setup_OkState()
    {
        _command = new CreateFooLinkCommand(Guid.NewGuid(), "Test title", "www");
        _projectValidatorMock = new Mock<IProjectValidator>();
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(x => x.FooExistsAsync(_command.FooGuid, default))
            .ReturnsAsync(true);
        _dut = new CreateFooLinkCommandValidator(_projectValidatorMock.Object, _fooValidatorMock.Object);
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
}
