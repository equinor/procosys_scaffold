using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Query.FooQueries;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public abstract class AccessValidatorForIFooQueryTests<TFooQuery> : AccessValidatorTestBase
    where TFooQuery : IBaseRequest, IIsFooQuery
{
    protected abstract TFooQuery GetFooCommandWithAccessToProject();
    protected abstract TFooQuery GetFooCommandWithoutAccessToProject();

    [TestMethod]
    public async Task Validate_ShouldReturnTrue_WhenAccessToProjectForFoo()
    {
        // Arrange
        var command = GetFooCommandWithAccessToProject();

        // act
        var result = await _dut.ValidateAsync(command);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task Validate_ShouldReturnFalse_WhenNoAccessToProjectForFoo()
    {
        // Arrange
        var command = GetFooCommandWithoutAccessToProject();

        // act
        var result = await _dut.ValidateAsync(command);

        // Assert
        Assert.IsFalse(result);
    }
}
