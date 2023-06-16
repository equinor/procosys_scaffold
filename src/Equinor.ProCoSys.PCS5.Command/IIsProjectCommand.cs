using MediatR;

namespace Equinor.ProCoSys.PCS5.Command;

public interface IIsProjectCommand : IBaseRequest
{
    string ProjectName { get; }
}
