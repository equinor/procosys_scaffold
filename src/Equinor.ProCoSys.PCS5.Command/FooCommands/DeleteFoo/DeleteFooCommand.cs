using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo
{
    public class DeleteFooCommand : IRequest<Result<Unit>>, IFooCommandRequest
    {
        public DeleteFooCommand(int fooId, string? rowVersion)
        {
            FooId = fooId;
            RowVersion = rowVersion;
        }

        public int FooId { get; }
        public string? RowVersion { get; }
    }
}
