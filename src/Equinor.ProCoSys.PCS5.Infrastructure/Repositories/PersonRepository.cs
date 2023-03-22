using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(PCS5Context context)
            : base(context, context.Persons) { }

        public Task<Person?> GetByOidAsync(Guid oid)
            => DefaultQuery.SingleOrDefaultAsync(p => p.Oid == oid);
    }
}
