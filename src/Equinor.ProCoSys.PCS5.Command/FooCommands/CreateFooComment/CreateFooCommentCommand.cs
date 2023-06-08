using System;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooComment;

public class CreateFooCommentCommand : IRequest<Result<GuidAndRowVersion>>, IFooCommandRequest
{
    public CreateFooCommentCommand(Guid fooGuid, string text)
    {
        FooGuid = fooGuid;
        Text = text;
    }

    public Guid FooGuid { get; }
    public string Text { get; }
}
