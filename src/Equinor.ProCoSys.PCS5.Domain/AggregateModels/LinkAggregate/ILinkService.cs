using System;
using System.Threading;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

public interface ILinkService
{
    Task<Link> AddAsync(string sourceType, Guid sourceGuid, string title, string url, CancellationToken cancellationToken);
    Task<Link> GetAllAsync(Guid sourceGuid, CancellationToken cancellationToken);
    Task<Link> UpdateAsync(Guid guid, string title, string url, CancellationToken cancellationToken);
    Task<Link> DeleteAsync(Guid guid, CancellationToken cancellationToken);
}
