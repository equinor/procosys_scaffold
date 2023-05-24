using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

public class FooVoidedEvent : FooEvent
{
    public FooVoidedEvent(Foo foo) : base("Foo voided", foo)
    { }
}
