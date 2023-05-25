using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.LinkEvents;

public class LinkCreatedEventHandler : BaseEventHandler, INotificationHandler<LinkCreatedEvent>
{
    public LinkCreatedEventHandler(IPersonRepository personRepository) : base(personRepository)
    {
    }

    // todo unit test
    public async Task Handle(LinkCreatedEvent notification, CancellationToken cancellationToken)
    {
        var createdByOid = await GetCreatedByOidAsync(notification.Link);

        // ToDo Send event to the bus
        return;
    }
}
