using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.CreateFoo;

[TestClass]
public class CreateFooCommandValidatorTests
{
    private CreateFooCommandValidator _dut;
    private CreateFooCommand _command;
    private Mock<IProjectValidator> _projectValidatorMock;
    private readonly string _projectName = "Project name";
    private readonly string _title = "Test title";

    [TestInitialize]
    public void Setup_OkState()
    {
        _projectValidatorMock = new Mock<IProjectValidator>();
        _command = new CreateFooCommand(_title, _projectName);
        _dut = new CreateFooCommandValidator(_projectValidatorMock.Object);
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
    public async Task Validate_ShouldFail_When_ProjectIsClosed()
    {
        // Arrange
        _projectValidatorMock.Setup(x => x.IsClosed(_projectName, default))
            .ReturnsAsync(true);

        // Act
        var result = await _dut.ValidateAsync(_command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Project is closed!"));
    }
}
