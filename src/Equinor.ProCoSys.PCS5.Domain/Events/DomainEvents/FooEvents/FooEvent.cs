using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

public class FooEvent : DomainEvent
{
    public FooEvent(string displayName, Foo foo) : base(displayName) => Foo = foo;

    public Foo Foo { get; }
}
