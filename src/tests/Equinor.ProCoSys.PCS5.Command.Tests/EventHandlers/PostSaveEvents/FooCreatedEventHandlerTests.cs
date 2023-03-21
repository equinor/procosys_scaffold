using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.EventHandlers.PostSaveEvents;
using Equinor.ProCoSys.PCS5.Domain.Events.PostSave;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Command.Tests.EventHandlers.PostSaveEvents
{
    [TestClass]
    public class FooCreatedEventHandlerTests
    {
        [TestMethod]
        public async Task Handle_ShouldDoSomething()
        {
            // Arrange
            var objectGuid = Guid.NewGuid();
            const string Plant = "TestPlant";
            var fooCreatedEvent = new FooCreatedEvent(Plant, objectGuid);
            var dut = new FooCreatedEventHandler();

            // Act
            await dut.Handle(fooCreatedEvent, default);

            // Assert something
            Assert.IsTrue(true);
        }
    }
}
