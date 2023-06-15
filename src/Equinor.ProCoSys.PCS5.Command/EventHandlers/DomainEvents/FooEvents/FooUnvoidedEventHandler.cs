using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public class FooUnvoidedEventHandler : INotificationHandler<FooUnvoidedEvent>
{
    // todo unit test
    public Task Handle(FooUnvoidedEvent notification, CancellationToken cancellationToken)
    {
        var sourceGuid = notification.Foo.Guid;

        // ToDo Send event to the bus
        return Task.CompletedTask;
    }
}
