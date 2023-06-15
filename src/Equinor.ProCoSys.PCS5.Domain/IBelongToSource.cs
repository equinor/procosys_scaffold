using System;

namespace Equinor.ProCoSys.PCS5.Domain
{
    public interface IBelongToSource
    {
        string SourceType { get; }
        Guid SourceGuid { get; }
    }
}
