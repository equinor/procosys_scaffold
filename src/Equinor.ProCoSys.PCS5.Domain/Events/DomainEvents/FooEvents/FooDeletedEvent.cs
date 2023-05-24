using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

public class FooDeletedEvent : FooEvent
{
    public FooDeletedEvent(Foo foo) : base("Foo deleted", foo)
    { }
}
