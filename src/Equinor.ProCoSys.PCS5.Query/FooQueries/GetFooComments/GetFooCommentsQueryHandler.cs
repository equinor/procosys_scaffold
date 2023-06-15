using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Query.Comments;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooComments;

public class GetFooCommentsQueryHandler : IRequestHandler<GetFooCommentsQuery, Result<IEnumerable<CommentDto>>>
{
    private readonly ICommentService _commentService;

    public GetFooCommentsQueryHandler(ICommentService commentService) => _commentService = commentService;

    public async Task<Result<IEnumerable<CommentDto>>> Handle(GetFooCommentsQuery request, CancellationToken cancellationToken)
    {
        var commentDtos = await _commentService.GetAllForSourceAsync(request.FooGuid, cancellationToken);
        return new SuccessResult<IEnumerable<CommentDto>>(commentDtos);
    }
}
