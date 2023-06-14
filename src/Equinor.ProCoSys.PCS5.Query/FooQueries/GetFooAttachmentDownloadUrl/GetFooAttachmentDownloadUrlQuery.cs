using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooAttachmentDownloadUrl;

public class GetFooAttachmentDownloadUrlQuery : IRequest<Result<Uri>>, IFooQueryRequest
{
    public GetFooAttachmentDownloadUrlQuery(Guid fooGuid, Guid attachmentGuid)
    {
        FooGuid = fooGuid;
        AttachmentGuid = attachmentGuid;
    }

    public Guid FooGuid { get; }
    public Guid AttachmentGuid { get; }
}
