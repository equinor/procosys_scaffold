using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Query.FooQueries;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public abstract class AccessValidatorForIIsFooQueryTests<TFooQuery> : AccessValidatorTestBase
    where TFooQuery : IBaseRequest, IIsFooQuery
{
    protected abstract TFooQuery GetFooQueryWithAccessToProject();
    protected abstract TFooQuery GetFooQueryWithoutAccessToProject();

    [TestMethod]
    public async Task Validate_ShouldReturnTrue_WhenAccessToProjectForFoo()
    {
        // Arrange
        var command = GetFooQueryWithAccessToProject();

        // act
        var result = await _dut.ValidateAsync(command);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task Validate_ShouldReturnFalse_WhenNoAccessToProjectForFoo()
    {
        // Arrange
        var command = GetFooQueryWithoutAccessToProject();

        // act
        var result = await _dut.ValidateAsync(command);

        // Assert
        Assert.IsFalse(result);
    }
}
