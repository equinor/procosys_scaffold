using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public class FooCreatedEventHandler : BaseEventHandler, INotificationHandler<FooCreatedEvent>
{
    public FooCreatedEventHandler(IPersonRepository personRepository) : base(personRepository)
    {
    }

    // todo unit test
    public async Task Handle(FooCreatedEvent notification, CancellationToken cancellationToken)
    {
        var createdByOid = await GetCreatedByOidAsync(notification.Foo);

        // ToDo Send event to the bus
        return;
    }
}
