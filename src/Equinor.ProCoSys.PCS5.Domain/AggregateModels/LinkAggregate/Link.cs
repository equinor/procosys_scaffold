using System;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Time;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Audit;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

public class Link : EntityBase, IAggregateRoot, ICreationAuditable, IModificationAuditable
{
    public const int TitleLengthMax = 256;
    public const int UrlLengthMax = 2000;

    public Link(Guid sourceGuid, string title, string url)
    {
        Title = title;
        Url = url;
        Guid = Guid.NewGuid();
        SourceGuid = sourceGuid;
    }

    // private set needed for EntityFramework
    public Guid SourceGuid { get; private set; }
    public string Title { get; private set; }
    public string Url { get; set; }
    public DateTime CreatedAtUtc { get; private set; }
    public int CreatedById { get; private set; }
    public DateTime? ModifiedAtUtc { get; private set; }
    public int? ModifiedById { get; private set; }

    public void SetCreated(Person createdBy)
    {
        CreatedAtUtc = TimeService.UtcNow;
        if (createdBy == null)
        {
            throw new ArgumentNullException(nameof(createdBy));
        }
        CreatedById = createdBy.Id;
    }

    public void SetModified(Person modifiedBy)
    {
        ModifiedAtUtc = TimeService.UtcNow;
        if (modifiedBy == null)
        {
            throw new ArgumentNullException(nameof(modifiedBy));
        }
        ModifiedById = modifiedBy.Id;
    }
}
