using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFooLink;

public class DeleteFooLinkCommand : IRequest<Result<Unit>>, IIsFooCommand
{
    public DeleteFooLinkCommand(Guid fooGuid, Guid linkGuid, string rowVersion)
    {
        FooGuid = fooGuid;
        LinkGuid = linkGuid;
        RowVersion = rowVersion;
    }

    public Guid FooGuid { get; }
    public Guid LinkGuid { get; }
    public string RowVersion { get; }
}
