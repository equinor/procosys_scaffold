using System;
using System.Collections.Generic;
using Equinor.ProCoSys.PCS5.Query.Attachments;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooAttachments;

public class GetFooAttachmentsQuery : IRequest<Result<IEnumerable<AttachmentDto>>>, IFooQueryRequest
{
    public GetFooAttachmentsQuery(Guid fooGuid) => FooGuid = fooGuid;

    public Guid FooGuid { get; }
}
