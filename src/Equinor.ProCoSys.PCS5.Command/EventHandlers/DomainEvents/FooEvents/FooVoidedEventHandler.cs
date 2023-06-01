using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public class FooVoidedEventHandler : BaseEventHandler, INotificationHandler<FooVoidedEvent>
{
    public FooVoidedEventHandler(IPersonRepository personRepository) : base(personRepository)
    {
    }

    // todo unit test
    public async Task Handle(FooVoidedEvent notification, CancellationToken cancellationToken)
    {
        var modifiedByOid = await GetModifiedByOidAsync(notification.Foo);
        var sourceGuid = notification.Foo.Guid;

        // ToDo Send event to the bus
        return;
    }
}
