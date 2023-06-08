using System;

namespace Equinor.ProCoSys.PCS5.Query.Comments;

public class CommentDto
{
    public CommentDto(
        Guid sourceGuid,
        Guid guid,
        string text,
        PersonDto createdBy,
        DateTime createdAtUtc,
        string rowVersion)
    {
        SourceGuid = sourceGuid;
        Guid = guid;
        Text = text;
        CreatedBy = createdBy;
        CreatedAtUtc = createdAtUtc;
        RowVersion = rowVersion;
    }

    public Guid SourceGuid { get; }
    public Guid Guid { get; }
    public string Text { get; }
    public PersonDto CreatedBy { get; }
    public DateTime CreatedAtUtc { get; }
    public string RowVersion { get; }
}
