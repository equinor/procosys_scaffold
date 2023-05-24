using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.LinkEvents;

public abstract class LinkEventHandler
{
    public Task Handle(LinkEvent notification, CancellationToken cancellationToken)
    {
        // ToDo Send event to the bus
        return Task.CompletedTask;
    }
}
