using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Application.Dtos;

namespace Equinor.ProCoSys.PCS5.Application.Interfaces;

public interface ILinkService
{
    Task<LinkDto> AddAsync(
        string sourceType,
        Guid sourceGuid,
        string title,
        string url,
        CancellationToken cancellationToken);

    Task<IEnumerable<LinkDto>> GetAllForSourceAsync(
        Guid sourceGuid,
        CancellationToken cancellationToken);
    
    Task<string> UpdateAsync(
        Guid guid,
        string title,
        string url,
        string rowVersion,
        CancellationToken cancellationToken);
    
    Task DeleteAsync(
        Guid guid,
        string rowVersion,
        CancellationToken cancellationToken);
    
    Task<bool> ExistsAsync(
        Guid guid,
        CancellationToken cancellationToken);
    
    Task<LinkDto?> TryGetAsync(
        Guid guid,
        CancellationToken cancellationToken);
}
