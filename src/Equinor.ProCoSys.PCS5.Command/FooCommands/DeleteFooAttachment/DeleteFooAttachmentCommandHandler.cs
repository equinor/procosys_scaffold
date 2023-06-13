using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFooAttachment;

public class DeleteFooAttachmentCommandHandler : IRequestHandler<DeleteFooAttachmentCommand, Result<Unit>>
{
    private readonly IAttachmentService _attachmentService;

    public DeleteFooAttachmentCommandHandler(IAttachmentService attachmentService) => _attachmentService = attachmentService;

    public async Task<Result<Unit>> Handle(DeleteFooAttachmentCommand request, CancellationToken cancellationToken)
    {
        await _attachmentService.DeleteAsync(
            request.AttachmentGuid,
            request.RowVersion,
            cancellationToken);

        return new SuccessResult<Unit>(Unit.Value);
    }
}
