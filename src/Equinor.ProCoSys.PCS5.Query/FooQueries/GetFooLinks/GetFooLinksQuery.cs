using System;
using System.Collections.Generic;
using Equinor.ProCoSys.PCS5.Query.Links;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooLinks;

public class GetFooLinksQuery : IRequest<Result<IEnumerable<LinkDto>>>, IIsFooQuery
{
    public GetFooLinksQuery(Guid fooGuid) => FooGuid = fooGuid;

    public Guid FooGuid { get; }
}
