﻿namespace Equinor.ProCoSys.PCS5.Query.GetFooById
{
    public class FooDto
    {
        public FooDto(
            int id,
            string projectName,
            string title,
            PersonDto createdBy,
            string rowVersion)
        {
            Id = id;
            ProjectName = projectName;
            Title = title;
            CreatedBy = createdBy;
            RowVersion = rowVersion;
        }

        public int Id { get; }
        public string ProjectName { get; }
        public string Title { get; }
        public PersonDto CreatedBy { get; }
        public string RowVersion { get; }
    }
}
