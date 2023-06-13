using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFooAttachment;

public class DeleteFooAttachmentCommand : IRequest<Result<Unit>>, IFooCommandRequest
{
    public DeleteFooAttachmentCommand(Guid fooGuid, Guid attachmentGuid, string rowVersion)
    {
        FooGuid = fooGuid;
        AttachmentGuid = attachmentGuid;
        RowVersion = rowVersion;
    }

    public Guid FooGuid { get; }
    public Guid AttachmentGuid { get; }
    public string RowVersion { get; }
}
