using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories;

public class ProjectRepository : EntityWithGuidRepository<Project>, IProjectRepository
{
    public ProjectRepository(PCS5Context context)
        : base(context, context.Projects, context.Projects)
            
    {
    }

    public Task<Project?> TryGetProjectByNameAsync(string projectName)
        => DefaultQuery.SingleOrDefaultAsync(p => !string.IsNullOrEmpty(projectName) &&  p.Name == projectName);
}
