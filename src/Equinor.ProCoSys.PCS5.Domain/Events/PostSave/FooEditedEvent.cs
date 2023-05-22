using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PostSave;

public class FooEditedEvent : IPostSaveDomainEvent
{
    public FooEditedEvent(Guid guid) => Guid = guid;
        
    public Guid Guid { get; }
}
