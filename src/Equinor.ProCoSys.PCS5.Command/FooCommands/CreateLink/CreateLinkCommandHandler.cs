using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Equinor.ProCoSys.PCS5.Domain;
using MediatR;
using ServiceResult;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateLink;

public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, Result<GuidAndRowVersion>>
{
    private readonly ILogger<CreateLinkCommandHandler> _logger;

    private readonly ILinkRepository _linkRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLinkCommandHandler(
        ILinkRepository linkRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateLinkCommandHandler> logger)
    {
        _linkRepository = linkRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    // todo create unit test
    public async Task<Result<GuidAndRowVersion>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        var link = new Link(request.FooGuid, request.Title, request.Url);
        _linkRepository.Add(link);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Link '{request.Title}' created");

        return new SuccessResult<GuidAndRowVersion>(new GuidAndRowVersion(link.Guid, link.RowVersion.ConvertToString()));
    }
}
