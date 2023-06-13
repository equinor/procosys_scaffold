using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.AttachmentEvents;

public class ExistingAttachmentUploadedAndOverwrittenEvent : AttachmentEvent
{
    public ExistingAttachmentUploadedAndOverwrittenEvent(Attachment attachment)
        : base($"Existing attachment uploaded again for {attachment.SourceType}", attachment)
    {
    }
}
