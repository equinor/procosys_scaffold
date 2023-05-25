using System.Threading.Tasks;
using System;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories;

public class LinkRepository : RepositoryBase<Link>, ILinkRepository
{
    public LinkRepository(PCS5Context context)
        : base(context, context.Links, context.Links)
    {
    }

    public Task<List<Link>> GetAllForSourceAsync(Guid sourceGuid)
        => DefaultQuery.Where(l => l.SourceGuid == sourceGuid).ToListAsync();
}
