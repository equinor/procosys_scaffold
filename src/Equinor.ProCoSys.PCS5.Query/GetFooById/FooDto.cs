namespace Equinor.ProCoSys.PCS5.Query.GetFooById
{
    public class FooDto
    {
        public FooDto(
            string projectName,
            string title,
            PersonDto createdBy,
            string rowVersion)
        {
            ProjectName = projectName;
            Title = title;
            CreatedBy = createdBy;
            RowVersion = rowVersion;
        }

        public string ProjectName { get; }
        public string Title { get; }
        public PersonDto CreatedBy { get; }
        public string RowVersion { get; }
    }
}
