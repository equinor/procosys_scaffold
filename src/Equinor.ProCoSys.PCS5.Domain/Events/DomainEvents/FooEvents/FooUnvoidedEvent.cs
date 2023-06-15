using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

public class FooUnvoidedEvent : FooEvent
{
    public FooUnvoidedEvent(Foo foo) : base("Foo unvoided", foo)
    { }
}
