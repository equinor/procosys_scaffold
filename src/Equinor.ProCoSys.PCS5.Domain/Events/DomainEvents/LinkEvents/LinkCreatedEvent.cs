using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;

public class LinkCreatedEvent : LinkEvent
{
    public LinkCreatedEvent(Link link)
        : base($"Link created for {link.SourceType}", link)
    {
    }
}
