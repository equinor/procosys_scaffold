using System;
using System.Collections.Generic;
using Equinor.ProCoSys.PCS5.Query.Comments;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooComments;

public class GetFooCommentsQuery : IRequest<Result<IEnumerable<CommentDto>>>, IFooQueryRequest
{
    public GetFooCommentsQuery(Guid fooGuid) => FooGuid = fooGuid;

    public Guid FooGuid { get; }
}
