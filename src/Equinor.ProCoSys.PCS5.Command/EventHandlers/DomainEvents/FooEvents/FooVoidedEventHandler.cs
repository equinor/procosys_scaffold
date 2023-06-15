using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public class FooVoidedEventHandler : INotificationHandler<FooVoidedEvent>
{
    // todo unit test
    public Task Handle(FooVoidedEvent notification, CancellationToken cancellationToken)
    {
        var sourceGuid = notification.Foo.Guid;

        // ToDo Send event to the bus
        return Task.CompletedTask;
    }
}
