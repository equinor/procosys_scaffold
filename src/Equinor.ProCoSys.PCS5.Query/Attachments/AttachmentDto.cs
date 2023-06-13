using System;

namespace Equinor.ProCoSys.PCS5.Query.Attachments;

public class AttachmentDto
{
    public AttachmentDto(
        Guid sourceGuid,
        Guid guid,
        string fileName,
        PersonDto createdBy,
        DateTime createdAtUtc,
        PersonDto? modifiedBy,
        DateTime? modifiedAtUtc,
        string rowVersion)
    {
        SourceGuid = sourceGuid;
        Guid = guid;
        FileName = fileName;
        CreatedBy = createdBy;
        CreatedAtUtc = createdAtUtc;
        ModifiedBy = modifiedBy;
        ModifiedAtUtc = modifiedAtUtc;
        RowVersion = rowVersion;
    }

    public Guid SourceGuid { get; }
    public Guid Guid { get; }
    public string FileName { get; }
    public PersonDto CreatedBy { get; }
    public DateTime CreatedAtUtc { get; set; }
    public PersonDto? ModifiedBy { get; }
    public DateTime? ModifiedAtUtc { get; set; }
    public string RowVersion { get; }
}
