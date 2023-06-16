using System;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries;

public interface IIsFooQuery : IBaseRequest
{
    Guid FooGuid { get; }
}
