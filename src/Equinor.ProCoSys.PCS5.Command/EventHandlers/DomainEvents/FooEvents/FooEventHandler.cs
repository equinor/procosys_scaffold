using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public abstract class FooEventHandler
{
    public Task Handle(FooEvent notification, CancellationToken cancellationToken)
    {
        // ToDo Send event to the bus
        return Task.CompletedTask;
    }
}
