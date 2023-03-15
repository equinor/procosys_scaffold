using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.Events.PostSave;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.PostSaveEvents
{
    public class FooCreatedEventHandler : INotificationHandler<FooCreatedEvent>
    {
        public Task Handle(FooCreatedEvent notification, CancellationToken cancellationToken)
        {
            // to something which should happen after a Foo is created and saved
            return Task.CompletedTask;
        }
    }
}
