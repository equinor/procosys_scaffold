using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.PostSave;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.PostSaveEvents;

public class FooLinkCreatedEventHandler : INotificationHandler<LinkCreatedEvent>
{
    public Task Handle(LinkCreatedEvent notification, CancellationToken cancellationToken)
    {
        // ToDo do something which should happen after a Foo Link is created and saved
        return Task.CompletedTask;
    }
}
