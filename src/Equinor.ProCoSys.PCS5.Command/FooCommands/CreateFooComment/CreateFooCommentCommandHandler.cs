using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Comments;
using MediatR;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooComment;

public class CreateFooCommentCommandHandler : IRequestHandler<CreateFooCommentCommand, Result<GuidAndRowVersion>>
{
    private readonly ICommentService _commentService;

    public CreateFooCommentCommandHandler(ICommentService commentService) => _commentService = commentService;

    public async Task<Result<GuidAndRowVersion>> Handle(CreateFooCommentCommand request, CancellationToken cancellationToken)
    {
        var commentDto = await _commentService.AddAsync(nameof(Foo), request.FooGuid, request.Text, cancellationToken);

        return new SuccessResult<GuidAndRowVersion>(new GuidAndRowVersion(commentDto.Guid, commentDto.RowVersion));
    }
}
