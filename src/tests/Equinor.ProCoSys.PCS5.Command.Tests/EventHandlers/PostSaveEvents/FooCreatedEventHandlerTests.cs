using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.EventHandlers.PostSaveEvents;

[TestClass]
public class FooCreatedEventHandlerTests
{
    [TestMethod]
    public async Task Handle_ShouldDoSomething()
    {
        // Arrange
        var personOid = Guid.NewGuid();
        var foo = new Foo("X", new Project("X", Guid.NewGuid(), "Pro", "Desc"), "F");
        var fooCreatedEvent = new FooCreatedEvent(foo);
        var personRepoositoryMock = new Mock<IPersonRepository>();
        personRepoositoryMock.Setup(p => p.GetByIdAsync(foo.CreatedById))
            .ReturnsAsync(new Person(personOid, "P", "S", "ps", "ps@pcs.com"));
        var dut = new FooCreatedEventHandler(personRepoositoryMock.Object);

        // Act
        await dut.Handle(fooCreatedEvent, default);

        // ToDo Assert something
        Assert.IsTrue(true);
    }
}
