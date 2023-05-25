using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.VoidFoo;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.VoidFoo;

[TestClass]
public class VoidFooCommandValidatorTests
{
    private readonly Guid _fooGuid = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private VoidFooCommandValidator _dut;
    private Mock<IFooValidator> _fooValidatorMock;

    private VoidFooCommand _command;

    [TestInitialize]
    public void Setup_OkState()
    {
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(x => x.FooExistsAsync(_fooGuid, default)).ReturnsAsync(true);
        _command = new VoidFooCommand(_fooGuid, _rowVersion);

        _dut = new VoidFooCommandValidator(_fooValidatorMock.Object);
    }

    [TestMethod]
    public async Task Validate_ShouldBeValid_WhenOkState()
    {
        var result = await _dut.ValidateAsync(_command);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public async Task Validate_ShouldFail_When_FooAlreadyVoided()
    {
        _fooValidatorMock.Setup(x => x.FooIsVoidedAsync(_fooGuid, default)).ReturnsAsync(true);

        var result = await _dut.ValidateAsync(_command);

        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Foo is already voided!"));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_When_FooNotExists()
    {
        _fooValidatorMock.Setup(inv => inv.FooExistsAsync(_fooGuid, default))
            .ReturnsAsync(false);

        var result = await _dut.ValidateAsync(_command);

        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Foo with this guid does not exist!"));
    }
}
