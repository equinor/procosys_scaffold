using System;

namespace Equinor.ProCoSys.PCS5.Command.Attachments;

public class AttachmentDto
{
    public AttachmentDto(Guid guid, string rowVersion)
    {
        Guid = guid;
        RowVersion = rowVersion;
    }

    public Guid Guid { get; }
    public string RowVersion { get; }
}
