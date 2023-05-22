using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PreSave;

public class FooCreatingEvent : IPreSaveDomainEvent
{
    public FooCreatingEvent(string plant, Guid guid)
    {
        Plant = plant;
        Guid = guid;
    }
    public string Plant { get; }
    public Guid Guid { get; }
}
