using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Query.Comments;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetAllForSourceAsync(
        Guid sourceGuid,
        CancellationToken cancellationToken);
}
