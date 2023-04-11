namespace Equinor.ProCoSys.PCS5.Query.GetFooById;

public class FooDetailsDto
{
    public FooDetailsDto(
        int id,
        string projectName,
        string title,
        string? text,
        PersonDto createdBy,
        bool isVoided,
        string rowVersion)
    {
        Id = id;
        ProjectName = projectName;
        Title = title;
        Text = text;
        CreatedBy = createdBy;
        IsVoided = isVoided;
        RowVersion = rowVersion;
    }

    public int Id { get; }
    public string ProjectName { get; }
    public string Title { get; }
    public string? Text { get; }
    public PersonDto CreatedBy { get; }
    public bool IsVoided { get; }
    public string RowVersion { get; }
}
