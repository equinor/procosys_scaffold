using System;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries;

public interface IFooQueryRequest
{
    Guid FooGuid { get; }
}
