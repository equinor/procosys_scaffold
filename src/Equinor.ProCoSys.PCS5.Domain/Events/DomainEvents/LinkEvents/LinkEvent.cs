using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;

public abstract class LinkEvent : DomainEvent
{
    protected LinkEvent(string displayName, Link link) : base(displayName) => Link = link;

    public Link Link { get; }
}
