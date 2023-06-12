using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.Projects.GetOpenProjects;

public class GetOpenProjectsQueryHandler : IRequestHandler<GetOpenProjectsQuery, Result<List<ProjectDto>>>
{
    private readonly IReadOnlyContext _context;

    public GetOpenProjectsQueryHandler(IReadOnlyContext context) => _context = context;

    public async Task<Result<List<ProjectDto>>> Handle(
        GetOpenProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var projectDtos = await (from pro in _context.QuerySet<Project>()
                                 where !pro.IsClosed
                                 select new ProjectDto(
                                     pro.Id,
                                     pro.Name,
                                     pro.Description)
            )
            .TagWith($"{nameof(GetOpenProjectsQueryHandler)}.{nameof(Handle)}")
            .ToListAsync(cancellationToken);

        return new SuccessResult<List<ProjectDto>>(projectDtos);
    }
}
