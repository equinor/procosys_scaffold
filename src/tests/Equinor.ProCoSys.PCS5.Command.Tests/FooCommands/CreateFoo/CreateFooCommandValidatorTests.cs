using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.CreateFoo;

[TestClass]
public class CreateFooCommandValidatorTests
{
    private CreateFooCommandValidator _dut;
    private CreateFooCommand _command;
    private readonly string _projectName = "Project name";
    private readonly string _title = "Test title";

    [TestInitialize]
    public void Setup_OkState()
    {
        _command = new CreateFooCommand(_title, _projectName);
        _dut = new CreateFooCommandValidator();
    }

    [TestMethod]
    public async Task Validate_ShouldBeValid_WhenOkState()
    {
        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsTrue(result.IsValid);
    }
}
