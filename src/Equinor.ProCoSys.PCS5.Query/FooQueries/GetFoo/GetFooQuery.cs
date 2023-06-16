using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoo;

public class GetFooQuery : IRequest<Result<FooDetailsDto>>, IIsFooQuery
{
    public GetFooQuery(Guid fooGuid) => FooGuid = fooGuid;

    public Guid FooGuid { get; }
}
