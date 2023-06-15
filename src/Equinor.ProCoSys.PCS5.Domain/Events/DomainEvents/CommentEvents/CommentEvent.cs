using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.CommentAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.CommentEvents;

public abstract class CommentEvent : DomainEvent
{
    protected CommentEvent(string displayName, Comment comment) : base(displayName) => Comment = comment;

    public Comment Comment { get; }
}
