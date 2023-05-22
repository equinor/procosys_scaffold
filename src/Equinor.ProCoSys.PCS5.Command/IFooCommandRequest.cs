using System;

namespace Equinor.ProCoSys.PCS5.Command;

public interface IFooCommandRequest
{
    Guid FooGuid { get; }
}
