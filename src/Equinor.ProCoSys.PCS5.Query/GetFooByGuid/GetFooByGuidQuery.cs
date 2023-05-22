using System;
using Equinor.ProCoSys.PCS5.Query.GetFooByGuid;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.GetFooByGuid;

public class GetFooByGuidQuery : IRequest<Result<FooDetailsDto>>, IFooQueryRequest
{
    public GetFooByGuidQuery(Guid fooGuid) => FooGuid = fooGuid;

    public Guid FooGuid { get; }
}
