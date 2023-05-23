using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PostSave;

public class FooVoidedEvent : IPostSaveDomainEvent
{
    public FooVoidedEvent(Guid guid) => Guid = guid;
        
    public Guid Guid { get; }
}
