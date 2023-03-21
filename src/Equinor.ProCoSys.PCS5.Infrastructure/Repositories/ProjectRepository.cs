using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(PCS5Context context)
            : base(context, context.Projects, context.Projects)
            
        {
        }

        public Task<Project> GetProjectOnlyByNameAsync(string projectName)
            => Set.SingleOrDefaultAsync(p => p.Name == projectName);
    }
}
