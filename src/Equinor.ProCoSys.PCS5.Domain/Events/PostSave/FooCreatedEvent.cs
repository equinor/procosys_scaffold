using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PostSave;

public class FooCreatedEvent : IPostSaveDomainEvent
{
    public FooCreatedEvent(string plant, Guid guid)
    {
        Plant = plant;
        Guid = guid;
    }
    public string Plant { get; }
    public Guid Guid { get; }
}
