using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Controllers.Foo;

[TestClass]
public class CreateFooDtoValidatorTests
{
    private readonly CreateFooDtoValidator _dut = new();

    [TestMethod]
    public async Task Validate_ShouldBeValid_WhenOkState()
    {
        // Arrange
        var dto = new CreateFooDto { Title = "New title", ProjectName = "P" };
        
        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public async Task Validate_ShouldFail_WhenTitleNotGiven()
    {
        // Arrange
        var dto = new CreateFooDto { ProjectName = "P" };

        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("'Title' must not be empty."));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_WhenTitleIsTooShort()
    {
        // Arrange
        var dto = new CreateFooDto { Title = "N", ProjectName = "P" };

        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("The length of 'Title' must be"));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_WhenTitleIsTooLongAsync()
    {
        // Arrange
        var dto = new CreateFooDto
        {
            Title = new string('x', Domain.AggregateModels.FooAggregate.Foo.TitleLengthMax + 1),
            ProjectName = "P"
        };

        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("The length of 'Title' must be"));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_WhenProjectNotGiven()
    {
        // Arrange
        var dto = new CreateFooDto { Title = "New title" };

        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("'Project Name' must not be empty."));
    }
}
