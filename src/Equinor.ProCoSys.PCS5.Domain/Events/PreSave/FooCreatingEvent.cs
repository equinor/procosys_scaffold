using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PreSave;

public class FooCreatingEvent : IPreSaveDomainEvent
{
    public FooCreatingEvent(Guid guid) => Guid = guid;
    
    public Guid Guid { get; }
}
