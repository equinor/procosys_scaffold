using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.WebApi.Controllers;
using Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Controllers.Foo;

[TestClass]
public class EditFooDtoValidatorTests
{
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private EditFooDtoValidator _dut;
    private Mock<IRowVersionValidator> _rowVersionValidatorMock;

    [TestInitialize]
    public void Setup_OkState()
    {
        _rowVersionValidatorMock = new Mock<IRowVersionValidator>();
        _rowVersionValidatorMock.Setup(x => x.IsValid(_rowVersion)).Returns(true);

        _dut = new EditFooDtoValidator(_rowVersionValidatorMock.Object);
    }

    [TestMethod]
    public async Task Validate_ShouldBeValid_WhenOkState()
    {
        // Arrange
        var dto = new EditFooDto { Title = "New title", RowVersion = _rowVersion };
        
        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public async Task Validate_ShouldFail_WhenTitleNotGiven()
    {
        // Arrange
        var dto = new EditFooDto { RowVersion = _rowVersion };

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
        var dto = new EditFooDto { Title = "N", Text = "New text", RowVersion = _rowVersion };

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
        var dto = new EditFooDto
        {
            Title = new string('x', Domain.AggregateModels.FooAggregate.Foo.TitleLengthMax + 1),
            RowVersion = _rowVersion
        };

        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("The length of 'Title' must be"));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_WhenRowVersionNotGiven()
    {
        // Arrange
        var dto = new EditFooDto { Title = "New title" };

        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("'Row Version' must not be empty."));
    }

    [TestMethod]
    public async Task Validate_ShouldFail_WhenIllegalRowVersion()
    {
        // Arrange
        _rowVersionValidatorMock.Setup(x => x.IsValid(_rowVersion)).Returns(false);
        var dto = new EditFooDto { Title = "New title", RowVersion = _rowVersion };

        // Act
        var result = await _dut.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Dto does not have valid rowVersion!"));
    }
}
