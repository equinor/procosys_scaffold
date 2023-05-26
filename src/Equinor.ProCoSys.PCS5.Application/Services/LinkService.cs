using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Application.Dtos;
using Equinor.ProCoSys.PCS5.Application.Interfaces;
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

    public LinkService(
        ILinkRepository linkRepository,
        IUnitOfWork unitOfWork,
        ILogger<LinkService> logger)
    {
        _linkRepository = linkRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<LinkDto> AddAsync(
        string sourceType,
        Guid sourceGuid,
        string title,
        string url,
        CancellationToken cancellationToken)
    {
        var link = new Link(sourceType, sourceGuid, title, url);
        _linkRepository.Add(link);
        link.AddDomainEvent(new LinkCreatedEvent(link));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug($"Link '{link.Title}' with guid {link.Guid} created for {link.SourceGuid}");

        return new LinkDto(link.Guid, link.Title, link.Url, link.RowVersion.ConvertToString());
    }

    public async Task<bool> ExistsAsync(
        Guid guid,
        CancellationToken cancellationToken)
        => await TryGetAsync(guid, cancellationToken) != null;

    public async Task<IEnumerable<LinkDto>> GetAllForSourceAsync(
        Guid sourceGuid,
        CancellationToken cancellationToken)
    {
        var links = await _linkRepository.GetAllForSourceAsync(sourceGuid);

        var linkDtos = links
            .Select(l => new LinkDto(l.Guid, l.Title, l.Url, l.RowVersion.ConvertToString()));

        return linkDtos;
    }

    public async Task<LinkDto?> TryGetAsync(
        Guid guid,
        CancellationToken cancellationToken)
    {
        var link = await _linkRepository.TryGetByGuidAsync(guid);

        if (link == null)
        {
            return null;
        }

        return new LinkDto(link.Guid, link.Title, link.Url, link.RowVersion.ConvertToString());
    }

    public async Task<string> UpdateAsync(
        Guid guid,
        string title,
        string url,
        string rowVersion,
        CancellationToken cancellationToken)
    {
        var link = await _linkRepository.TryGetByGuidAsync(guid);

        if (link == null)
        {
            throw new Exception($"Link with guid {guid} not found when updating");
        }

        link.SetRowVersion(rowVersion);
        link.Url = url;
        link.Title = title;
        link.AddDomainEvent(new LinkUpdatedEvent(link));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug($"Link '{link.Title}' with guid {link.Guid} updated for {link.SourceGuid}");

        return link.RowVersion.ConvertToString();
    }

    public async Task DeleteAsync(
        Guid guid,
        string rowVersion,
        CancellationToken cancellationToken)
    {
        var link = await _linkRepository.TryGetByGuidAsync(guid);

        if (link == null)
        {
            throw new Exception($"Link with guid {guid} not found when updating");
        }

        // Setting RowVersion before delete has 2 missions:
        // 1) Set correct Concurrency
        // 2) Trigger the update of modifiedBy / modifiedAt to be able to log who performed the deletion
        link.SetRowVersion(rowVersion);
        _linkRepository.Remove(link);
        link.AddDomainEvent(new LinkDeletedEvent(link));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug($"Link '{link.Title}' with guid {link.Guid} deleted for {link.SourceGuid}");

        return;
    }
}
