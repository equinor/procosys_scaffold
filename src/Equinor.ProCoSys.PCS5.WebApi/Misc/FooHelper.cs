using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.WebApi.Misc;

public class FooHelper : IFooHelper
{
    private readonly IReadOnlyContext _context;

    public FooHelper(IReadOnlyContext context) => _context = context;

    public async Task<string?> GetProjectNameAsync(int fooId)
    {
        var projectName = await (from p in _context.QuerySet<Project>()
            join foo in _context.QuerySet<Foo>() on p.Id equals foo.ProjectId
            where foo.Id == fooId
            select p.Name).SingleOrDefaultAsync();

        return projectName;
    }
}