using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PostSave;

public class FooEditedEvent : IPostSaveDomainEvent
{
    public FooEditedEvent(Guid proCoSysGuid) => ProCoSysGuid = proCoSysGuid;
        
    public Guid ProCoSysGuid { get; }
}
