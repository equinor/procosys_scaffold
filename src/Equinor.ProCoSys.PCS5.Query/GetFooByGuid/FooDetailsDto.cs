using System;

namespace Equinor.ProCoSys.PCS5.Query.GetFooByGuid;

public class FooDetailsDto
{
    public FooDetailsDto(
        Guid guid,
        string projectName,
        string title,
        string? text,
        PersonDto createdBy,
        bool isVoided,
        string rowVersion)
    {
        Guid = guid;
        ProjectName = projectName;
        Title = title;
        Text = text;
        CreatedBy = createdBy;
        IsVoided = isVoided;
        RowVersion = rowVersion;
    }

    public Guid Guid { get; }
    public string ProjectName { get; }
    public string Title { get; }
    public string? Text { get; }
    public PersonDto CreatedBy { get; }
    public bool IsVoided { get; }
    public string RowVersion { get; }
}
