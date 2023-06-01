using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command.EventHandlers.DomainEvents.FooEvents;

public class FooDeletedEventHandler : BaseEventHandler, INotificationHandler<FooDeletedEvent>
{
    public FooDeletedEventHandler(IPersonRepository personRepository) : base(personRepository)
    {
    }

    // todo unit test
    public async Task Handle(FooDeletedEvent notification, CancellationToken cancellationToken)
    {
        // When deleting, both ModifiedBy and ModifiedAtUtc are set to be the deleter / deletion time
        var deletedByOid = await GetModifiedByOidAsync(notification.Foo);
#pragma warning disable CS8629 // Nullable value type may be null.
        var deletedAtUtc = notification.Foo.ModifiedAtUtc.Value;
#pragma warning restore CS8629 // Nullable value type may be null.
        var sourceGuid = notification.Foo.Guid;

        // ToDo Send event to the bus
        return;
    }
}
