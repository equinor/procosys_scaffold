using System;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PreSave;

public class FooEditedEvent : INotification
{
    public FooEditedEvent(Guid proCoSysGuid) => ProCoSysGuid = proCoSysGuid;
        
    public Guid ProCoSysGuid { get; }
}