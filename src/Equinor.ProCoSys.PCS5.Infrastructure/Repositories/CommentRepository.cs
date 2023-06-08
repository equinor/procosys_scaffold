using Equinor.ProCoSys.PCS5.Domain.AggregateModels.CommentAggregate;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories;

public class CommentRepository : RepositoryBaseWithGuid<Comment>, ICommentRepository
{
    public CommentRepository(PCS5Context context)
        : base(context, context.Comments, context.Comments)
    {
    }
}
