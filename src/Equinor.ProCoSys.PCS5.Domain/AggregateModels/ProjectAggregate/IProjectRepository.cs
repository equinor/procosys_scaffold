using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;

public interface IProjectRepository : IRepositoryWithGuid<Project>
{
    Task<Project?> TryGetProjectByNameAsync(string projectName);
}
