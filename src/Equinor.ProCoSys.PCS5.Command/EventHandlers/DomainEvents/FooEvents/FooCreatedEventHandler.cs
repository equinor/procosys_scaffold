using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public class FooCreatedEventHandler : FooEventHandler, INotificationHandler<FooCreatedEvent>
{
    public Task Handle(FooCreatedEvent notification, CancellationToken cancellationToken)
        => base.Handle(notification, cancellationToken);
}
