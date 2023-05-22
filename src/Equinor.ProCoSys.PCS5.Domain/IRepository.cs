using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain;

public interface IRepository<TEntity> where TEntity : EntityBase, IAggregateRoot
{
    void Add(TEntity item);

    Task<bool> Exists(int id);

    Task<TEntity?> GetByIdAsync(int id);

    Task<TEntity?> GetByGuidAsync(Guid guid);

    Task<List<TEntity>> GetByIdsAsync(IEnumerable<int> id);

    void Remove(TEntity entity);

    Task<List<TEntity>> GetAllAsync();

}
