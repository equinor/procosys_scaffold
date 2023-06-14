using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Query.Attachments;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooAttachmentDownloadUrl;

public class GetFooAttachmentDownloadUrlQueryHandler : IRequestHandler<GetFooAttachmentDownloadUrlQuery, Result<Uri>>
{
    private readonly IAttachmentService _attachmentService;

    public GetFooAttachmentDownloadUrlQueryHandler(IAttachmentService attachmentService) => _attachmentService = attachmentService;

    // todo unit test
    public async Task<Result<Uri>> Handle(GetFooAttachmentDownloadUrlQuery request, CancellationToken cancellationToken)
    {
        var uri = await _attachmentService.TryGetDownloadUriAsync(request.AttachmentGuid, cancellationToken);
        if (uri == null)
        {
            return new NotFoundResult<Uri>($"Attachment with Guid {request.AttachmentGuid} not found");
        }

        return new SuccessResult<Uri>(uri);
    }
}
