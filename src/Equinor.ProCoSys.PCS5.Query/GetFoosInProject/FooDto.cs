using System;

namespace Equinor.ProCoSys.PCS5.Query.GetFoosInProject;

public class FooDto
{
    public FooDto(
        Guid guid,
        string projectName,
        string title,
        bool isVoided,
        string rowVersion)
    {
        Guid = guid;
        ProjectName = projectName;
        Title = title;
        IsVoided = isVoided;
        RowVersion = rowVersion;
    }

    public Guid Guid { get; }
    public string ProjectName { get; }
    public string Title { get; }
    public bool IsVoided { get; }
    public string RowVersion { get; }
}
