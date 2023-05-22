using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.PostSave;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.PostSaveEvents;

public class LinkCreatedEventHandler : INotificationHandler<LinkCreatedEvent>
{
    public Task Handle(LinkCreatedEvent notification, CancellationToken cancellationToken)
    {
        // ToDo do something which should happen after a Foo Link is created and saved
        if (notification.Title.Contains("XXX"))
        {
            throw new Exception("Can't add event");
        }

        return Task.CompletedTask;
    }
}
