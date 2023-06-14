using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Query.Attachments;

public interface IAttachmentService
{
    Task<IEnumerable<AttachmentDto>> GetAllForSourceAsync(
        Guid sourceGuid,
        CancellationToken cancellationToken);

    Task<Uri?> TryGetDownloadUriAsync(
        Guid guid,
        CancellationToken cancellationToken);
}
