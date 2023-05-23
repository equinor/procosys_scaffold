using System;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Audit;
using Equinor.ProCoSys.Common.Time;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.Events.PostSave;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

public class Foo : PlantEntityBase, IAggregateRoot, ICreationAuditable, IModificationAuditable, IVoidable
{
    private bool _isVoided;

    public const int TitleLengthMin = 3;
    public const int TitleLengthMax = 250;
    public const int TextLengthMax = 500;

#pragma warning disable CS8618
    protected Foo()
#pragma warning restore CS8618
        : base(null)
    {
    }

    public Foo(string plant, Project project, string title)
        : base(plant)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        if (project.Plant != plant)
        {
            throw new ArgumentException($"Can't relate item in {project.Plant} to item in {plant}");
        }
        ProjectId = project.Id;

        Title = title;

        Guid = Guid.NewGuid();

        AddPostSaveDomainEvent(new FooCreatedEvent(Guid));
    }

    // private set needed for EntityFramework
    public int ProjectId { get; private set; }
    public string Title { get; set; }
    public string? Text { get; set; }

    public DateTime CreatedAtUtc { get; private set; }
    public int CreatedById { get; private set; }
    public DateTime? ModifiedAtUtc { get; private set; }
    public int? ModifiedById { get; private set; }

    public void EditFoo(string title, string? text)
    {

        Title = title;
        Text = text;

        AddPostSaveDomainEvent(new FooEditedEvent(Guid));
    }

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

    public bool IsVoided
    {
        get => _isVoided;
        set
        {
            // do nothing if already set
            if (_isVoided == value)
            {
                return;
            }

            _isVoided = value;
            if (_isVoided)
            {
                AddPostSaveDomainEvent(new FooVoidedEvent(Guid));
            }
            else
            {
                AddPostSaveDomainEvent(new FooUnvoidedEvent(Guid));
            }
        }
    }
}
