using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoosInProject;

public class GetFoosInProjectQueryHandler : IRequestHandler<GetFoosInProjectQuery, Result<IEnumerable<FooDto>>>
{
    private readonly IReadOnlyContext _context;

    public GetFoosInProjectQueryHandler(IReadOnlyContext context) => _context = context;

    public async Task<Result<IEnumerable<FooDto>>> Handle(GetFoosInProjectQuery request, CancellationToken cancellationToken)
    {
        var foos =
            await (from foo in _context.QuerySet<Foo>()
                   join pro in _context.QuerySet<Project>()
                       on foo.ProjectId equals pro.Id
                   where pro.Name == request.ProjectName && (!foo.IsVoided || request.IncludeVoided)
                   select new FooDto(
                       foo.Guid,
                       pro.Name,
                       foo.Title,
                       foo.IsVoided,
                       foo.RowVersion.ConvertToString())
                )
                .TagWith($"{nameof(GetFoosInProjectQueryHandler)}.{nameof(Handle)}")
                .ToListAsync(cancellationToken);

        return new SuccessResult<IEnumerable<FooDto>>(foos);
    }
}
