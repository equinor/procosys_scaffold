using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Command.Tests.EventHandlers.DomainEvents;

[TestClass]
public class FooCreatedEventHandlerTests
{
    // ToDo Rename to better test name
    [TestMethod]
    public async Task Handle_ShouldDoSomething()
    {
        // Arrange
        var foo = new Foo("X", new Project("X", Guid.NewGuid(), "Pro", "Desc"), "F");
        var fooCreatedEvent = new FooCreatedEvent(foo);
        var dut = new FooCreatedEventHandler();

        // Act
        await dut.Handle(fooCreatedEvent, default);

        // ToDo Assert something
        Assert.IsTrue(true);
    }
}
