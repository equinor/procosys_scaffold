using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.EventHandlers.PostSaveEvents;
using Equinor.ProCoSys.PCS5.Domain.Events.PostSave;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Command.Tests.EventHandlers.PostSaveEvents;

[TestClass]
public class FooCreatedEventHandlerTests
{
    [TestMethod]
    public async Task Handle_ShouldDoSomething()
    {
        // Arrange
        var objectGuid = Guid.NewGuid();
        var fooCreatedEvent = new FooCreatedEvent(objectGuid);
        var dut = new FooCreatedEventHandler();

        // Act
        await dut.Handle(fooCreatedEvent, default);

        // ToDo Assert something
        Assert.IsTrue(true);
    }
}
