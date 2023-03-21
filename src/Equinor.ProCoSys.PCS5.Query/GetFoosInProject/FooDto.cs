namespace Equinor.ProCoSys.PCS5.Query.GetFoosInProject
{
    public class FooDto
    {
        public FooDto(
            int id,
            string projectName,
            string title,
            bool isVoided,
            string rowVersion)
        {
            Id = id;
            ProjectName = projectName;
            Title = title;
            IsVoided = isVoided;
            RowVersion = rowVersion;
        }

        public int Id { get; }
        public string ProjectName { get; }
        public string Title { get; }
        public bool IsVoided { get; }
        public string RowVersion { get; }
    }
}
