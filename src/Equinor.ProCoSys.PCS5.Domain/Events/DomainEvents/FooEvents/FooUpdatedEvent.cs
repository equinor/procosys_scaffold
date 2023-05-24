using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

public class FooUpdatedEvent : FooEvent
{
    public FooUpdatedEvent(Foo foo) : base("Foo updated", foo)
    { }
}
