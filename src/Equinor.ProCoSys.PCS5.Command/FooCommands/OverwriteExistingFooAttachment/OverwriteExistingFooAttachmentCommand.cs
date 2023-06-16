using System;
using System.IO;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.OverwriteExistingFooAttachment;

public class OverwriteExistingFooAttachmentCommand : UploadAttachmentCommand, IRequest<Result<string>>, IIsFooCommand
{
    public OverwriteExistingFooAttachmentCommand(Guid fooGuid, string fileName, string rowVersion, Stream content)
        : base(content)
    {
        FooGuid = fooGuid;
        FileName = fileName;
        RowVersion = rowVersion;
    }

    public Guid FooGuid { get; }

    public string FileName { get; }
    public string RowVersion { get; }
}
