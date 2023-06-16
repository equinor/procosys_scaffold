using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooLink;

public class CreateFooLinkCommand : IRequest<Result<GuidAndRowVersion>>, IIsFooCommand
{
    public CreateFooLinkCommand(Guid fooGuid, string title, string url)
    {
        FooGuid = fooGuid;
        Title = title;
        Url = url;
    }

    public Guid FooGuid { get; }
    public string Title { get; }
    public string Url { get; }
}
