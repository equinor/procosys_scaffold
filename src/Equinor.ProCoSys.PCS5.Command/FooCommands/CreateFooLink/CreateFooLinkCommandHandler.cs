using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Application.Interfaces;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooLink;

public class CreateFooLinkCommandHandler : IRequestHandler<CreateFooLinkCommand, Result<GuidAndRowVersion>>
{
    private readonly ILinkService _linkService;

    public CreateFooLinkCommandHandler(ILinkService linkService) => _linkService = linkService;

    // todo create unit test
    public async Task<Result<GuidAndRowVersion>> Handle(CreateFooLinkCommand request, CancellationToken cancellationToken)
    {
        var linkDto = await _linkService.AddAsync("Foo", request.FooGuid, request.Title, request.Url, cancellationToken);

        return new SuccessResult<GuidAndRowVersion>(new GuidAndRowVersion(linkDto.Guid, linkDto.RowVersion));
    }
}
