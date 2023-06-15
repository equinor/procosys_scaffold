using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;

public class LinkUpdatedEvent : LinkEvent
{
    public LinkUpdatedEvent(Link link)
        : base($"Link updated for {link.SourceType}", link)
    {
    }
}
