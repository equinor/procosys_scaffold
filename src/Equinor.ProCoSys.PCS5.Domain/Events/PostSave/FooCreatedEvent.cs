using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PostSave;

public class FooCreatedEvent : IPostSaveDomainEvent
{
    public FooCreatedEvent(
        string plant,
        Guid proCoSysGuid)
    {
        Plant = plant;
        ProCoSysGuid = proCoSysGuid;
    }
    public string Plant { get; }
    public Guid ProCoSysGuid { get; }
}
