using System;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;

public interface IAttachmentRepository : IRepositoryWithGuid<Attachment>
{
    Task<Attachment?> TryGetAttachmentWithFilenameForSourceAsync(Guid sourceGuid, string fileName);
}
