using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.VoidFoo;

public class VoidFooCommand : IRequest<Result<string>>, IFooCommandRequest
{
    public VoidFooCommand(Guid fooGuid, string? rowVersion)
    {
        FooGuid = fooGuid;
        RowVersion = rowVersion;
    }

    public Guid FooGuid { get; }
    public string? RowVersion { get; }
}
