using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
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
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var link = new Link(sourceType, sourceGuid, title, url);
            _linkRepository.Add(link);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation($"Link '{title}' created for {sourceGuid}");

            return link;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(e, $"Error creating link '{title}' for {sourceGuid}");
            throw;
        }

    }

    public Task<Link> GetAllAsync(Guid sourceGuid, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Link> UpdateAsync(Guid guid, string title, string url, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Link> DeleteAsync(Guid guid, CancellationToken cancellationToken) => throw new NotImplementedException();
}
