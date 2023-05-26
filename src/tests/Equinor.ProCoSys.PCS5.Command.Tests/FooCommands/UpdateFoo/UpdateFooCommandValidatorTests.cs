using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFoo;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.UpdateFoo;

[TestClass]
public class UpdateFooCommandValidatorTests
{
    private readonly Guid _fooGuid = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private UpdateFooCommandValidator _dut;
    private Mock<IFooValidator> _fooValidatorMock;

    private UpdateFooCommand _command;

    [TestInitialize]
    public void Setup_OkState()
    {
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(x => x.FooExistsAsync(_fooGuid, default))
            .ReturnsAsync(true);
        _command = new UpdateFooCommand(_fooGuid, "New title", "New text", _rowVersion);

        _dut = new UpdateFooCommandValidator(_fooValidatorMock.Object);
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
        _fooValidatorMock.Setup(inv => inv.FooExistsAsync(_fooGuid, default))
            .ReturnsAsync(false);

        var result = await _dut.ValidateAsync(_command);

        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Foo with this guid does not exist!"));
    }
}
