using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.CreateFoo;

[TestClass]
public class CreateFooCommandValidatorTests
{
    private CreateFooCommandValidator _dut;
    private CreateFooCommand _command;
    private Mock<IFooValidator> _fooValidatorMock;
    private readonly string _projectName = "Project name";
    private readonly string _title = "Test title";

    [TestInitialize]
    public void Setup_OkState()
    {
        _fooValidatorMock = new Mock<IFooValidator>();
        _fooValidatorMock.Setup(inv => inv.FooIsOk()).Returns(true);
        _command = new CreateFooCommand(_title, _projectName);
        _dut = new CreateFooCommandValidator(_fooValidatorMock.Object);
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
}
