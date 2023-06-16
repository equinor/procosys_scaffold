using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFoo;

public class UpdateFooCommand : IRequest<Result<string>>, IIsFooCommand
{
    public UpdateFooCommand(Guid fooGuid, string title, string? text, string rowVersion)
    {
        FooGuid = fooGuid;
        Title = title;
        Text = text;
        RowVersion = rowVersion;
    }

    public Guid FooGuid { get; }
    public string Title { get; }
    public string? Text { get; }
    public string RowVersion { get; }
}
