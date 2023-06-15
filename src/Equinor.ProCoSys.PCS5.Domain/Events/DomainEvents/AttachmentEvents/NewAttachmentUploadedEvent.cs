using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.AttachmentEvents;

public class NewAttachmentUploadedEvent : AttachmentEvent
{
    public NewAttachmentUploadedEvent(Attachment attachment)
        : base($"New attachment uploaded for {attachment.SourceType}", attachment)
    {
    }
}
