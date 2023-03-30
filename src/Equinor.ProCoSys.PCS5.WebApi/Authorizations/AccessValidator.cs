using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Command;
using Equinor.ProCoSys.PCS5.Query;
using Equinor.ProCoSys.PCS5.WebApi.Misc;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Equinor.ProCoSys.PCS5.WebApi.Authorizations;

/// <summary>
/// Validates if current user has access to perform a request of type IProjectRequest, 
/// IFooCommandRequest or IFooQueryRequest.
/// It validates if user has access to the project of the request 
/// </summary>
public class AccessValidator : IAccessValidator
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IProjectAccessChecker _projectAccessChecker;
    private readonly IFooHelper _fooHelper;
    private readonly ILogger<AccessValidator> _logger;

    public AccessValidator(
        ICurrentUserProvider currentUserProvider,
        IProjectAccessChecker projectAccessChecker,
        IFooHelper fooHelper,
        ILogger<AccessValidator> logger)
    {
        _currentUserProvider = currentUserProvider;
        _projectAccessChecker = projectAccessChecker;
        _fooHelper = fooHelper;
        _logger = logger;
    }

    public async Task<bool> ValidateAsync<TRequest>(TRequest request) where TRequest : IBaseRequest
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var userOid = _currentUserProvider.GetCurrentUserOid();
        if (request is IProjectRequest projectRequest &&
            !_projectAccessChecker.HasCurrentUserAccessToProject(projectRequest.ProjectName))
        {
            _logger.LogWarning($"Current user {userOid} don't have access to project {projectRequest.ProjectName}");
            return false;
        }

        if (request is IFooCommandRequest fooCommandRequest)
        {
            if (!await HasCurrentUserAccessToProjectAsync(fooCommandRequest.FooId, userOid))
            {
                return false;
            }
        }

        if (request is IFooQueryRequest fooQueryRequest)
        {
            if (!await HasCurrentUserAccessToProjectAsync(fooQueryRequest.FooId, userOid))
            {
                return false;
            }
        }

        return true;
    }

    private async Task<bool> HasCurrentUserAccessToProjectAsync(int fooId, Guid userOid)
    {
        var projectName = await _fooHelper.GetProjectNameAsync(fooId);
        if (projectName != null)
        {
            var accessToProject = _projectAccessChecker.HasCurrentUserAccessToProject(projectName);

            if (!accessToProject)
            {
                _logger.LogWarning($"Current user {userOid} don't have access to project {projectName}");
                return false;
            }
        }

        return true;
    }
}
