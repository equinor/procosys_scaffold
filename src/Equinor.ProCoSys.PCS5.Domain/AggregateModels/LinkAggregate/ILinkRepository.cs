using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

public interface ILinkRepository : IRepository<Link>
{
    Task<List<Link>> GetAllForSourceAsync(Guid sourceGuid);
}
