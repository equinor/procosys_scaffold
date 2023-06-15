using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.UploadNewFooAttachment;

public class UploadNewFooAttachmentCommandHandler : IRequestHandler<UploadNewFooAttachmentCommand, Result<GuidAndRowVersion>>
{
    private readonly IAttachmentService _attachmentService;

    public UploadNewFooAttachmentCommandHandler(IAttachmentService attachmentService)
        => _attachmentService = attachmentService;

    public async Task<Result<GuidAndRowVersion>> Handle(UploadNewFooAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachmentDto = await _attachmentService.UploadNewAsync(
            nameof(Foo),
            request.FooGuid,
            request.FileName,
            request.Content,
            cancellationToken);

        return new SuccessResult<GuidAndRowVersion>(new GuidAndRowVersion(attachmentDto.Guid, attachmentDto.RowVersion));
    }
}
