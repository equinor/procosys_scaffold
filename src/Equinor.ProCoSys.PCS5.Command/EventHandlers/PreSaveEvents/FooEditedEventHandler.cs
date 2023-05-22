using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.PreSave;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.PreSaveEvents;

public class FooEditedEventHandler : INotificationHandler<FooEditingEvent>
{
    public Task Handle(FooEditingEvent notification, CancellationToken cancellationToken)
    {
        // ToDo do something which should happen after a Foo is edited but before it is saved
        return Task.CompletedTask;
    }
}
