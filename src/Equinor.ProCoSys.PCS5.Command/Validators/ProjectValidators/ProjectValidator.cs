using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators
{
    public class ProjectValidator : IProjectValidator
    {
        private readonly IReadOnlyContext _context;

        public ProjectValidator(IReadOnlyContext context) => _context = context;

        public async Task<bool> ExistsAsync(string projectName, CancellationToken cancellationToken) =>
            await (from p in _context.QuerySet<Project>()
                where p.Name == projectName
                select p).AnyAsync(cancellationToken);

        public async Task<bool> IsClosed(string projectName, CancellationToken cancellationToken)
        {
            var project = await (from p in _context.QuerySet<Project>()
                where p.Name == projectName
                select p).SingleOrDefaultAsync(cancellationToken);

            return project != null && project.IsClosed;
        }

        public async Task<bool> IsClosedForFoo(Guid fooGuid, CancellationToken cancellationToken)
        {
            var project = await (from foo in _context.QuerySet<Foo>()
                join p in _context.QuerySet<Project>() on foo.ProjectId equals p.Id
                where foo.Guid == fooGuid
                select p).SingleOrDefaultAsync(cancellationToken);

            return project != null && project.IsClosed;
        }
    }
}
