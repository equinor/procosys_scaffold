using System;
using System.IO;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.UploadNewFooAttachment;

public class UploadNewFooAttachmentCommand : UploadAttachmentCommand, IRequest<Result<GuidAndRowVersion>>, IIsFooCommand
{
    public UploadNewFooAttachmentCommand(Guid fooGuid, string fileName, Stream content)
        : base(content)
    {
        FooGuid = fooGuid;
        FileName = fileName;
    }

    public Guid FooGuid { get; }

    public string FileName { get; }
}
