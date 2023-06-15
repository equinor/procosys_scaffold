using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.LinkEvents;

public class CommentCreatedEventHandler : INotificationHandler<LinkCreatedEvent>
{
    // todo unit test
    public Task Handle(LinkCreatedEvent notification, CancellationToken cancellationToken)
    {
        var sourceGuid = notification.Link.SourceGuid;

        // ToDo Send event to the bus
        return Task.CompletedTask;
    }
}
