using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories;

public class LinkRepository : RepositoryBaseWithGuid<Link>, ILinkRepository
{
    public LinkRepository(PCS5Context context)
        : base(context, context.Links, context.Links)
    {
    }
}
