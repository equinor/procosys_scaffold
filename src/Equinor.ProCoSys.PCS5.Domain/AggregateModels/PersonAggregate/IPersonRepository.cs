using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person> GetByOidAsync(Guid oid);
    }
}
