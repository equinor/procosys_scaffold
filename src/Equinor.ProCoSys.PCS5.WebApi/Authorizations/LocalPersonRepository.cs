using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.Auth.Authorization;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;

namespace Equinor.ProCoSys.PCS5.WebApi.Authorizations
{
    public class LocalPersonRepository : ILocalPersonRepository
    {
        private readonly IPersonRepository _personRepository;

        public LocalPersonRepository(IPersonRepository personRepository)
            => _personRepository = personRepository;

        public async Task<bool> ExistsAsync(Guid userOid)
            => await _personRepository.GetByOidAsync(userOid) != null;
    }
}
