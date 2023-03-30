using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.EditFoo;

public class EditFooCommand : IRequest<Result<string>>, IFooCommandRequest
{
    public EditFooCommand(int fooId, string? title, string? rowVersion)
    {
        FooId = fooId;
        Title = title;
        RowVersion = rowVersion;
    }

    public int FooId { get; }
    public string? Title { get; }
    public string? RowVersion { get; }
}