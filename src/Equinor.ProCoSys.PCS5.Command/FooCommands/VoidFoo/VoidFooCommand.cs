using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.VoidFoo
{
    public class VoidFooCommand : IRequest<Result<string>>, IFooCommandRequest
    {
        public VoidFooCommand(int fooId, string rowVersion)
        {
            FooId = fooId;
            RowVersion = rowVersion;
        }

        public int FooId { get; }
        public string RowVersion { get; }
    }
}
