using System;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PreSave
{
    public class FooCreatedEvent : INotification
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
}
