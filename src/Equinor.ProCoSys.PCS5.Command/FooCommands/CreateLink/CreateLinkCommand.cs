using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateLink;

public class CreateLinkCommand : IRequest<Result<GuidAndRowVersion>>, IFooCommandRequest
{
    public CreateLinkCommand(Guid fooGuid, string title, string url)
    {
        FooGuid = fooGuid;
        Title = title;
        Url = url;
    }

    public Guid FooGuid { get; }
    public string Title { get; }
    public string Url { get; }
}
