using System.Threading.Tasks;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project> GetProjectOnlyByNameAsync(string projectName);
}
