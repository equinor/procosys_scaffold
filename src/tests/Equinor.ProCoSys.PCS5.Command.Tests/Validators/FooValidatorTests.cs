using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Command.Tests.Validators;

[TestClass]
public class FooValidatorTests : ReadOnlyTestsBase
{
    private Guid _nonVoidedFooGuid;
    private Guid _voidedFooGuid;

    protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
    {
        using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
            
        var foo1 = new Foo(TestPlantA, _projectA, "Foo 1");
        var foo2 = new Foo(TestPlantA, _projectA, "Foo 2") { IsVoided = true };
        context.Foos.Add(foo1);
        context.Foos.Add(foo2);
        context.SaveChangesAsync().Wait();
        _nonVoidedFooGuid = foo1.Guid;
        _voidedFooGuid = foo2.Guid;
    }

    #region FooExists
    [TestMethod]
    public async Task FooExists_ForExistingFoo_ShouldReturnTrue()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);            
        var dut = new FooValidator(context);

        // Act
        var result = await dut.FooExistsAsync(_nonVoidedFooGuid, default);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task FooExists_ForNonExistingFoo_ShouldReturnFalse()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);    
        var dut = new FooValidator(context);

        // Act
        var result = await dut.FooExistsAsync(Guid.Empty, default);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion

    #region FooIsVoided
    [TestMethod]
    public async Task FooIsVoided_ForVoidedFoo_ShouldReturnTrue()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);    
        var dut = new FooValidator(context);

        // Act
        var result = await dut.FooIsVoidedAsync(_voidedFooGuid, default);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task FooIsVoided_ForNonVoidedFoo_ShouldReturnFalse()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new FooValidator(context);

        // Act
        var result = await dut.FooExistsAsync(_nonVoidedFooGuid, default);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task FooIsVoided_ForNonExistingFoo_ShouldReturnFalse()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);  
        var dut = new FooValidator(context);

        // Act
        var result = await dut.FooExistsAsync(Guid.NewGuid(), default);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion
}
