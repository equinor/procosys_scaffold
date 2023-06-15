using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;

public class LinkDeletedEvent : LinkEvent
{
    public LinkDeletedEvent(Link link)
        : base($"Link deleted for {link.SourceType}", link)
    {
    }
}
