using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.Common.Time;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Equinor.ProCoSys.Common.Misc;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Tests;

[TestClass]
public class UnitOfWorkTests
{
    private readonly string _plant = "PCS$TESTPLANT";
    private Project _project;
    private readonly Guid _currentUserOid = new("12345678-1234-1234-1234-123456789123");
    private readonly DateTime _currentTime = new(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc);

    private DbContextOptions<PCS5Context> _dbContextOptions;
    private Mock<IPlantProvider> _plantProviderMock;
    private Mock<IEventDispatcher> _eventDispatcherMock;
    private Mock<ICurrentUserProvider> _currentUserProviderMock;
    private ManualTimeProvider _timeProvider;

    [TestInitialize]
    public void Setup()
    {
        _project = new(_plant, Guid.NewGuid(), "Project", "Description of Project");

        _dbContextOptions = new DbContextOptionsBuilder<PCS5Context>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _plantProviderMock = new Mock<IPlantProvider>();
        _plantProviderMock.Setup(x => x.Plant)
            .Returns(_plant);

        _eventDispatcherMock = new Mock<IEventDispatcher>();

        _currentUserProviderMock = new Mock<ICurrentUserProvider>();

        _timeProvider = new ManualTimeProvider(_currentTime);
        TimeService.SetProvider(_timeProvider);
    }

    [TestMethod]
    public async Task SaveChangesAsync_SetsCreatedProperties_WhenCreated()
    {
        // Arrange
        await using var dut = new PCS5Context(_dbContextOptions, _plantProviderMock.Object, _eventDispatcherMock.Object, _currentUserProviderMock.Object);

        var user = new Person(_currentUserOid, "Current", "User", "cu", "cu@pcs.pcs");
        dut.Persons.Add(user);
        dut.SaveChanges();

        _currentUserProviderMock
            .Setup(x => x.GetCurrentUserOid())
            .Returns(_currentUserOid);
        var newFoo = new Foo(_plant, _project, "Title");
        dut.Foos.Add(newFoo);

        // Act
        await dut.SaveChangesAsync();

        // Assert
        Assert.AreEqual(_currentTime, newFoo.CreatedAtUtc);
        Assert.AreEqual(user.Id, newFoo.CreatedById);
        Assert.IsNull(newFoo.ModifiedAtUtc);
        Assert.IsNull(newFoo.ModifiedById);
    }

    [TestMethod]
    public async Task SaveChangesAsync_SetsModifiedProperties_WhenModified()
    {
        // Arrange
        await using var dut = new PCS5Context(_dbContextOptions, _plantProviderMock.Object, _eventDispatcherMock.Object, _currentUserProviderMock.Object);

        var user = new Person(_currentUserOid, "Current", "User", "cu", "cu@pcs.pcs");
        dut.Persons.Add(user);
        await dut.SaveChangesAsync();

        _currentUserProviderMock
            .Setup(x => x.GetCurrentUserOid())
            .Returns(_currentUserOid);

        var newFoo = new Foo(_plant, _project, "Title");
        dut.Foos.Add(newFoo);

        await dut.SaveChangesAsync();

        newFoo.Title = "UpdatedTitle";
            
        // Act
        await dut.SaveChangesAsync();

        // Assert
        Assert.AreEqual(_currentTime, newFoo.ModifiedAtUtc);
        Assert.AreEqual(user.Id, newFoo.ModifiedById);
    }
}