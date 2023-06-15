using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.OverwriteExistingFooAttachment;

public class OverwriteExistingFooAttachmentCommandHandler : IRequestHandler<OverwriteExistingFooAttachmentCommand, Result<string>>
{
    private readonly IAttachmentService _attachmentService;

    public OverwriteExistingFooAttachmentCommandHandler(IAttachmentService attachmentService)
        => _attachmentService = attachmentService;

    public async Task<Result<string>> Handle(OverwriteExistingFooAttachmentCommand request, CancellationToken cancellationToken)
    {
        var newRowVersion = await _attachmentService.UploadOverwriteAsync(
            nameof(Foo),
            request.FooGuid,
            request.FileName,
            request.Content,
            request.RowVersion,
            cancellationToken);

        return new SuccessResult<string>(newRowVersion);
    }
}
