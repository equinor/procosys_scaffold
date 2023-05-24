using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents;

public class FooEventHandler : INotificationHandler<FooEvent>
{
    public Task Handle(FooEvent notification, CancellationToken cancellationToken)
    {
        // ToDo do something which should happen after a Foo is created and saved
        return Task.CompletedTask;
    }
}
