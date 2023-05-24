using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public class FooDeletedEventHandler : FooEventHandler, INotificationHandler<FooDeletedEvent>
{
    public Task Handle(FooDeletedEvent notification, CancellationToken cancellationToken)
        => base.Handle(notification, cancellationToken);
}
