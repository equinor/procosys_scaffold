using System;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.Events.PostSave;

public class LinkCreatedEvent : IPostSaveDomainEvent
{
    public LinkCreatedEvent(string sourceType, Guid sourceGuid, Guid linkGuid, string title)
    {
        SourceType = sourceType;
        SourceGuid = sourceGuid;
        LinkGuid = linkGuid;
        Title = title;
    }
    
    public string SourceType { get; }
    public Guid SourceGuid { get; }
    public Guid LinkGuid { get; }
    public string Title { get; }
}
