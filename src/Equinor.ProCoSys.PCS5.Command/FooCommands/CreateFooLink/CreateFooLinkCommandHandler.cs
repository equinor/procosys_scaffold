using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Links;
using MediatR;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooLink;

public class CreateFooLinkCommandHandler : IRequestHandler<CreateFooLinkCommand, Result<GuidAndRowVersion>>
{
    private readonly ILinkService _linkService;

    public CreateFooLinkCommandHandler(ILinkService linkService) => _linkService = linkService;

    public async Task<Result<GuidAndRowVersion>> Handle(CreateFooLinkCommand request, CancellationToken cancellationToken)
    {
        var linkDto = await _linkService.AddAsync(nameof(Foo), request.FooGuid, request.Title, request.Url, cancellationToken);

        return new SuccessResult<GuidAndRowVersion>(new GuidAndRowVersion(linkDto.Guid, linkDto.RowVersion));
    }
}
