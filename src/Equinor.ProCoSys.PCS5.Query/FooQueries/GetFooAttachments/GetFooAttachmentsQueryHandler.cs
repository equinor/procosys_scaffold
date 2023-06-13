using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Query.Attachments;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooAttachments;

public class GetFooAttachmentsQueryHandler : IRequestHandler<GetFooAttachmentsQuery, Result<IEnumerable<AttachmentDto>>>
{
    private readonly IAttachmentService _attachmentService;

    public GetFooAttachmentsQueryHandler(IAttachmentService attachmentService) => _attachmentService = attachmentService;

    public async Task<Result<IEnumerable<AttachmentDto>>> Handle(GetFooAttachmentsQuery request, CancellationToken cancellationToken)
    {
        var attachmentDtos = await _attachmentService.GetAllForSourceAsync(request.FooGuid, cancellationToken);
        return new SuccessResult<IEnumerable<AttachmentDto>>(attachmentDtos);
    }
}
