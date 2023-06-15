using System;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories;

public abstract class EntityWithGuidRepository<TEntity> : EntityRepository<TEntity> where TEntity : EntityBase, IAggregateRoot, IHaveGuid
{
    protected EntityWithGuidRepository(PCS5Context context, DbSet<TEntity> set)
        : this(context, set, set)
    {
    }

    protected EntityWithGuidRepository(PCS5Context context, DbSet<TEntity> set, IQueryable<TEntity> defaultQuery)
    : base(context, set, defaultQuery)
    {
    }

    public virtual Task<TEntity?> TryGetByGuidAsync(Guid guid) =>
        DefaultQuery.SingleOrDefaultAsync(x => x.Guid == guid);
}
