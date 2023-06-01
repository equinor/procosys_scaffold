using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Links;
using Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFooLink;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.UpdateFooLink;

[TestClass]
public class UpdateFooLinkCommandValidatorTests
{
    private readonly Guid _fooGuid = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private readonly Guid _linkGuid = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaab");
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private UpdateFooLinkCommandValidator _dut;
    private Mock<IFooValidator> _fooValidatorMock;
    private Mock<ILinkService> _linkServiceMock;

    private UpdateFooLinkCommand _command;

    [TestInitialize]
    public void Setup_OkState()
    {
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(x => x.FooExistsAsync(_fooGuid, default))
            .ReturnsAsync(true);
        _linkServiceMock = new Mock<ILinkService>();
        _linkServiceMock.Setup(x => x.ExistsAsync(_linkGuid, default))
            .ReturnsAsync(true);
        _command = new UpdateFooLinkCommand(_fooGuid, _linkGuid, "New title", "New text", _rowVersion);

        _dut = new UpdateFooLinkCommandValidator(_fooValidatorMock.Object, _linkServiceMock.Object);
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
        _fooValidatorMock.Setup(inv => inv.FooExistsAsync(_fooGuid, default))
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
        _linkServiceMock.Setup(x => x.ExistsAsync(_linkGuid, default))
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
        _fooValidatorMock.Setup(inv => inv.FooIsVoidedAsync(_fooGuid, default))
            .ReturnsAsync(true);

        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Foo is voided!"));
    }
}
