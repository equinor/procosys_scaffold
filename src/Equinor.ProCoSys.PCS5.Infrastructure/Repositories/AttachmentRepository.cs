using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories;

public class AttachmentRepository : EntityWithGuidRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(PCS5Context context)
        : base(context, context.Attachments, context.Attachments)
    {
    }
}
