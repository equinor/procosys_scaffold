using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents;

public class FooDeletedEventHandler : INotificationHandler<FooDeletedEvent>
{
    public Task Handle(FooDeletedEvent notification, CancellationToken cancellationToken)
    {
        // ToDo do something which should happen after a Foo is created and saved
        return Task.CompletedTask;
    }
}
