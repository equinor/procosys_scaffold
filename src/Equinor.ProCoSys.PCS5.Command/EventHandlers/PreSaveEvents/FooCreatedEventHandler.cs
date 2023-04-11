using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.PreSave;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.PreSaveEvents;

public class FooCreatedEventHandler : INotificationHandler<FooCreatingEvent>
{
    public Task Handle(FooCreatingEvent notification, CancellationToken cancellationToken)
    {
        // do something which should happen after a Foo is created but before it is saved
        return Task.CompletedTask;
    }
}
