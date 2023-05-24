using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Application.Dtos;
using Equinor.ProCoSys.PCS5.Application.Interfaces;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooLinks;

public class GetFooLinksQueryHandler : IRequestHandler<GetFooLinksQuery, Result<IEnumerable<LinkDto>>>
{
    private readonly ILinkService _linkService;

    public GetFooLinksQueryHandler(ILinkService linkService) => _linkService = linkService;

    public async Task<Result<IEnumerable<LinkDto>>> Handle(GetFooLinksQuery request, CancellationToken cancellationToken)
    {
        var linkDtos = await _linkService.GetAllAsync(request.FooGuid, cancellationToken);
        return new SuccessResult<IEnumerable<LinkDto>>(linkDtos);
    }
}
