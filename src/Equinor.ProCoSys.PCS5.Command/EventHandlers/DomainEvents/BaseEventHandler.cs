using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Audit;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents;

public abstract class BaseEventHandler
{
    private readonly IPersonRepository _personRepository;

    protected BaseEventHandler(IPersonRepository personRepository) => _personRepository = personRepository;

    internal async Task<Guid> GetModifiedByOidAsync(IModificationAuditable modificationAuditable)
    {
        if (!modificationAuditable.ModifiedById.HasValue)
        {
            throw new Exception("Trying to get modifiedBy on an unmodified object");
        }

        return await GetPersonOidAsync(modificationAuditable.ModifiedById.Value);
    }

    internal async Task<Guid> GetCreatedByOidAsync(ICreationAuditable creationAuditable)
        => await GetPersonOidAsync(creationAuditable.CreatedById);

    internal async Task<Guid> GetPersonOidAsync(int personId)
    {
        var person = await _personRepository.GetByIdAsync(personId);

        if (person is null)
        {
            throw new Exception($"Person with id {personId} don't exists in DB");
        }

        return person.Oid;
    }
}
