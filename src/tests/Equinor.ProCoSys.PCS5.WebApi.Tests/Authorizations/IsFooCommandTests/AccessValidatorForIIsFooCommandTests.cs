using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public abstract class AccessValidatorForIIsFooCommandTests<TFooCommand> : AccessValidatorTestBase
    where TFooCommand : IBaseRequest, IIsFooCommand
{
    protected abstract TFooCommand GetFooCommandWithAccessToProject();
    protected abstract TFooCommand GetFooCommandWithoutAccessToProject();

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
