using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

public class FooCreatedEvent : FooEvent
{
    public FooCreatedEvent(Foo foo) : base("Foo created", foo)
    { }
}
