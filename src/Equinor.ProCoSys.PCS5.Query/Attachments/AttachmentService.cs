using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Query.Attachments;

public class AttachmentService : IAttachmentService
{
    private readonly IReadOnlyContext _context;

    public AttachmentService(IReadOnlyContext context) => _context = context;

    // todo unit test
    public async Task<IEnumerable<AttachmentDto>> GetAllForSourceAsync(
        Guid sourceGuid,
        CancellationToken cancellationToken)
    {
        var attachments =
            await (from a in _context.QuerySet<Attachment>()
                    join createdByUser in _context.QuerySet<Person>()
                        on a.CreatedById equals createdByUser.Id
                    from modifiedByUser in _context.QuerySet<Person>()
                        .Where(p => p.Id == a.ModifiedById).DefaultIfEmpty() //left join!
                   where a.SourceGuid == sourceGuid
                   select new AttachmentDto(
                       a.SourceGuid,
                       a.Guid,
                       a.FileName,
                       new PersonDto(
                           createdByUser.Guid,
                           createdByUser.FirstName,
                           createdByUser.LastName,
                           createdByUser.UserName,
                           createdByUser.Email),
                       a.CreatedAtUtc,
                       modifiedByUser != null ? 
                           new PersonDto(
                               modifiedByUser.Guid,
                               modifiedByUser.FirstName,
                               modifiedByUser.LastName,
                               modifiedByUser.UserName,
                               modifiedByUser.Email) : null, 
                       a.ModifiedAtUtc,
                       a.RowVersion.ConvertToString()
               ))
                .TagWith($"{nameof(AttachmentService)}.{nameof(GetAllForSourceAsync)}")
                .ToListAsync(cancellationToken);

        return attachments;
    }
}
