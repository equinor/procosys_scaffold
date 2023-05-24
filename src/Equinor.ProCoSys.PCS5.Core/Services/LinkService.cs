using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;
using Microsoft.Extensions.Logging;

namespace Equinor.ProCoSys.PCS5.Application.Services;

public class LinkService : ILinkService
{
    private readonly ILinkRepository _linkRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LinkService> _logger;

    public LinkService(ILinkRepository linkRepository, IUnitOfWork unitOfWork, ILogger<LinkService> logger)
    {
        _linkRepository = linkRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    // todo create unit test
    public async Task<Link> AddAsync(string sourceType, Guid sourceGuid, string title, string url, CancellationToken cancellationToken)
    {
        var link = new Link(sourceType, sourceGuid, title, url);
        _linkRepository.Add(link);
        link.AddDomainEvent(new LinkCreatedEvent(link));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Link '{title}' created for {sourceGuid}");

        return link;
    }

    public Task<Link> GetAllAsync(Guid sourceGuid, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Link> UpdateAsync(Guid guid, string title, string url, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Link> DeleteAsync(Guid guid, CancellationToken cancellationToken) => throw new NotImplementedException();
}
