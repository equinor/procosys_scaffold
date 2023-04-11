﻿using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.EditFoo;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.EditFoo;

[TestClass]
public class EditFooCommandValidatorTests
{
    private readonly int _fooId = 1;
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private EditFooCommandValidator _dut;
    private Mock<IFooValidator> _fooValidatorMock;

    private EditFooCommand _command;

    [TestInitialize]
    public void Setup_OkState()
    {
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(x => x.FooIsOk()).Returns(true);
        _fooValidatorMock.Setup(x => x.FooExistsAsync(_fooId, default))
            .ReturnsAsync(true);
        _command = new EditFooCommand(_fooId, "New title", "New text", _rowVersion);

        _dut = new EditFooCommandValidator(_fooValidatorMock.Object);
    }

    [TestMethod]
    public async Task Validate_ShouldBeValid_WhenOkState()
    {
        var result = await _dut.ValidateAsync(_command);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public async Task Validate_ShouldFail_When_FooNotOk()
    {
        _fooValidatorMock.Setup(inv => inv.FooIsOk()).Returns(false);

        var result = await _dut.ValidateAsync(_command);

        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Not a OK Foo!"));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_When_FooNotExists()
    {
        _fooValidatorMock.Setup(inv => inv.FooExistsAsync(_fooId, default))
            .ReturnsAsync(false);

        var result = await _dut.ValidateAsync(_command);

        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Foo with this ID does not exist!"));
    }
}
