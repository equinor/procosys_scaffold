using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories;

public class AttachmentRepository : EntityWithGuidRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(PCS5Context context)
        : base(context, context.Attachments, context.Attachments)
    {
    }

    // todo unit test
    public Task<Attachment?> TryGetAttachmentWithFilenameForSourceAsync(Guid sourceGuid, string fileName)
        => DefaultQuery.SingleOrDefaultAsync(a => a.SourceGuid == sourceGuid && a.FileName == fileName);
}
