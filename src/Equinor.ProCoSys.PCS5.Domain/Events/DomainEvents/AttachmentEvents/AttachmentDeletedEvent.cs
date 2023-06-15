using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.AttachmentEvents;

public class AttachmentDeletedEvent : AttachmentEvent
{
    public AttachmentDeletedEvent(Attachment attachment)
        : base($"Attachment deleted for {attachment.SourceType}", attachment)
    {
    }
}
