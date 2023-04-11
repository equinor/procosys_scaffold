using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PreSave;

public class FooCreatingEvent : IPreSaveDomainEvent
{
    public FooCreatingEvent(
        string plant,
        Guid proCoSysGuid)
    {
        Plant = plant;
        ProCoSysGuid = proCoSysGuid;
    }
    public string Plant { get; }
    public Guid ProCoSysGuid { get; }
}
