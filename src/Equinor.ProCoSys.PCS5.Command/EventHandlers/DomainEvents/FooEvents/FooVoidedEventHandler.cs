using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public class FooVoidedEventHandler : FooEventHandler, INotificationHandler<FooVoidedEvent>
{
    public Task Handle(FooVoidedEvent notification, CancellationToken cancellationToken)
        => base.Handle(notification, cancellationToken);
}
