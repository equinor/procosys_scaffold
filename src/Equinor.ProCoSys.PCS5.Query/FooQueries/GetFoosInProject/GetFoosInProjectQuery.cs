using System.Collections.Generic;
using Equinor.ProCoSys.PCS5.Command;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoosInProject;

// todo IIsProjectCommand should be IIsProjectQuery
public class GetFoosInProjectQuery : IRequest<Result<IEnumerable<FooDto>>>, IIsProjectCommand
{
    public GetFoosInProjectQuery(string projectName, bool includeVoided = false)
    {
        ProjectName = projectName;
        IncludeVoided = includeVoided;
    }

    public string ProjectName { get; }
    public bool IncludeVoided { get; }
}
