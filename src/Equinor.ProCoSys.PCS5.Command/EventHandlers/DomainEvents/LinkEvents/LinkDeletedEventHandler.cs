using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.LinkEvents;

public class LinkDeletedEventHandler : BaseEventHandler, INotificationHandler<LinkDeletedEvent>
{
    public LinkDeletedEventHandler(IPersonRepository personRepository) : base(personRepository)
    {
    }

    // todo unit test
    public async Task Handle(LinkDeletedEvent notification, CancellationToken cancellationToken)
    {
        var modifiedByOid = await GetModifiedByOidAsync(notification.Link);

        // ToDo Send event to the bus
        return;
    }
}
