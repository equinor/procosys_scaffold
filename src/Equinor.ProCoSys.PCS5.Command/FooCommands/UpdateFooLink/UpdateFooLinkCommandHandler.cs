using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Application.Interfaces;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFooLink;

public class UpdateFooLinkCommandHandler : IRequestHandler<UpdateFooLinkCommand, Result<string>>
{
    private readonly ILinkService _linkService;

    public UpdateFooLinkCommandHandler(ILinkService linkService) => _linkService = linkService;

    public async Task<Result<string>> Handle(UpdateFooLinkCommand request, CancellationToken cancellationToken)
    {
        var rowVersion = await _linkService.UpdateAsync(
            request.LinkGuid,
            request.Title,
            request.Url,
            request.RowVersion,
            cancellationToken);

        return new SuccessResult<string>(rowVersion);
    }
}
