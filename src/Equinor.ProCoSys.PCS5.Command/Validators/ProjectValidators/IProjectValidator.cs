using System;
using System.Threading;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators
{
    public interface IProjectValidator
    {
        Task<bool> ExistsAsync(string projectName, CancellationToken cancellationToken);
        Task<bool> IsClosed(string projectName, CancellationToken cancellationToken);
        Task<bool> IsClosedForFoo(Guid fooGuid, CancellationToken cancellationToken);
    }
}
