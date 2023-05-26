using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFooLink;

public class UpdateFooLinkCommand : IRequest<Result<string>>, IFooCommandRequest
{
    public UpdateFooLinkCommand(Guid fooGuid, Guid linkGuid, string title, string url, string rowVersion)
    {
        FooGuid = fooGuid;
        LinkGuid = linkGuid;
        Title = title;
        Url = url;
        RowVersion = rowVersion;
    }

    public Guid FooGuid { get; }
    public Guid LinkGuid { get; }
    public string Title { get; }
    public string Url { get; }
    public string RowVersion { get; }
}
