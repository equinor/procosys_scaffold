using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Application.Dtos;
using Equinor.ProCoSys.PCS5.Application.Interfaces;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Application.Services;

public class LinkService : ILinkService
{
    private readonly IReadOnlyContext _context;
    private readonly ILinkRepository _linkRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LinkService> _logger;

    public LinkService(
        IReadOnlyContext context,
        ILinkRepository linkRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<LinkService> logger)
    {
        _context = context;
        _linkRepository = linkRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    // todo create unit test
    public async Task<LinkDto> AddAsync(string sourceType, Guid sourceGuid, string title, string url, CancellationToken cancellationToken)
    {
        var link = new Link(sourceType, sourceGuid, title, url);
        _linkRepository.Add(link);
        link.AddDomainEvent(new LinkCreatedEvent(link));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Link '{title}' created for {sourceGuid}");

        return new LinkDto(link.Guid, link.Title, link.Url, link.RowVersion.ConvertToString());
    }

    public Task DeleteAsync(Guid guid, CancellationToken cancellationToken) => throw new NotImplementedException();
    
    public async Task<IEnumerable<LinkDto>> GetAllAsync(Guid sourceGuid, CancellationToken cancellationToken)
    {
        var links =
            await (from link in _context.QuerySet<Link>()
                  where link.SourceGuid == sourceGuid
                  select new LinkDto(link.Guid, link.Title, link.Url, link.RowVersion.ConvertToString())
                )
                .TagWith($"{nameof(LinkService)}: links")
                .ToListAsync(cancellationToken);

        return links;
    }

    public Task<string> UpdateAsync(Guid guid, string title, string url, CancellationToken cancellationToken) => throw new NotImplementedException();
}
