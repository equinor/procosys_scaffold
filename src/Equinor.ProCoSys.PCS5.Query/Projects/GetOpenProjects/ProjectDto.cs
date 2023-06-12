namespace Equinor.ProCoSys.PCS5.Query.Projects.GetOpenProjects;

public class ProjectDto
{
    public ProjectDto(
        int id,
        string name,
        string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
}
