using System;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Time;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Audit;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

public class Link : PlantEntityBase, IAggregateRoot, ICreationAuditable, IModificationAuditable
{
    public const int TitleLengthMax = 256;
    public const int UrlLengthMax = 2000;

#pragma warning disable CS8618
    protected Link()
#pragma warning restore CS8618
        : base(null)
    {
    }

    public Link(string plant, Guid proCoSysGuid, string title, string url)
        : base(plant)
    {
        ProCoSysGuid = proCoSysGuid;
        Title = title;
        Url = url;
    }

    // private set needed for EntityFramework
    public Guid ProCoSysGuid { get; private set; }
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
