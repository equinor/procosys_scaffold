using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.LinkEvents;

public class LinkCreatedEventHandler : LinkEventHandler, INotificationHandler<LinkCreatedEvent>
{
    public Task Handle(LinkCreatedEvent notification, CancellationToken cancellationToken)
        => base.Handle(notification, cancellationToken);
}
