using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;

namespace Equinor.ProCoSys.PCS5.Command.Tests.Validators;

[TestClass]
public class ProjectValidatorTests : ReadOnlyTestsBase
{
    private Project _openProject;
    private Project _closedProject;
    private Foo _fooInOpenProject;
    private Foo _fooInClosedProject;

    protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
    {
        using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
            
        _openProject = new Project(TestPlantA, Guid.NewGuid(), "Project 1", "D1");
        _closedProject = new Project(TestPlantA, Guid.NewGuid(), "Project 2", "D2") { IsClosed = true };
        context.Projects.Add(_openProject);
        context.Projects.Add(_closedProject);

        _fooInOpenProject = new Foo(TestPlantA, _openProject, "x1");
        _fooInClosedProject = new Foo(TestPlantA, _closedProject, "x2");
        context.Foos.Add(_fooInOpenProject);
        context.Foos.Add(_fooInClosedProject);

        context.SaveChangesAsync().Wait();
    }

    #region ExistsAsync
    [TestMethod]
    public async Task ExistsAsync_ForExistingClosedProject_ShouldReturnTrue()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);            
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.ExistsAsync(_closedProject.Name, default);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ExistsAsync_ForExistingOpenProject_ShouldReturnTrue()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.ExistsAsync(_openProject.Name, default);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ExistsAsync_ForNonExistingProject_ShouldReturnFalse()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);    
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.ExistsAsync("P X", default);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion

    #region IsClosed
    [TestMethod]
    public async Task IsClosed_ForClosedProject_ShouldReturnTrue()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.IsClosed(_closedProject.Name, default);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task IsClosed_ForOpenProject_ShouldReturnFalse()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.IsClosed(_openProject.Name, default);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task IsClosed_ForNonExistingProject_ShouldReturnFalse()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.IsClosed("P X", default);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion

    #region IsClosedForFoo
    [TestMethod]
    public async Task IsClosedForFoo_ForFooInClosedProject_ShouldReturnTrue()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.IsClosedForFoo(_fooInClosedProject.Guid, default);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task IsClosedForFoo_ForFooInOpenProject_ShouldReturnFalse()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.IsClosedForFoo(_fooInOpenProject.Guid, default);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task IsClosedForFoo_ForNonExistingFoo_ShouldReturnFalse()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new ProjectValidator(context);

        // Act
        var result = await dut.IsClosedForFoo(Guid.NewGuid(), default);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion
}
