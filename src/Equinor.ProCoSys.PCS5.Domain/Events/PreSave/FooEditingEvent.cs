using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PreSave;

public class FooEditingEvent : IPreSaveDomainEvent
{
    public FooEditingEvent(Guid proCoSysGuid) => ProCoSysGuid = proCoSysGuid;
        
    public Guid ProCoSysGuid { get; }
}
