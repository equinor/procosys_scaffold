using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PreSave;

public class FooDeletedEvent : IPreSaveDomainEvent
{
    public FooDeletedEvent(Guid guid) => Guid = guid;
    
    public Guid Guid { get; }
}
