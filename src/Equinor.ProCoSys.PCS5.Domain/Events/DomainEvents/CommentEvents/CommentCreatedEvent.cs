using Equinor.ProCoSys.PCS5.Domain.AggregateModels.CommentAggregate;

namespace Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.CommentEvents;

public class CommentCreatedEvent : CommentEvent
{
    public CommentCreatedEvent(Comment comment)
        : base($"Comment created for {comment.SourceType}", comment)
    {
    }
}
