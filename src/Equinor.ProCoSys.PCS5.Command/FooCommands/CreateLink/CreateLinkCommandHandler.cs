using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceResult;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateLink;

public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, Result<GuidAndRowVersion>>
{
    private readonly ILinkService _linkService;

    public CreateLinkCommandHandler(ILinkService linkService) => _linkService = linkService;

    // todo create unit test
    public async Task<Result<GuidAndRowVersion>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        var link = await _linkService.AddAsync("Foo", request.FooGuid, request.Title, request.Url, cancellationToken);

        return new SuccessResult<GuidAndRowVersion>(new GuidAndRowVersion(link.Guid, link.RowVersion.ConvertToString()));
    }
}
