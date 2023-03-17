using System.Collections.Generic;
using Equinor.ProCoSys.Common;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.GetFoosInProject
{
    public class GetFoosInProjectQuery : IRequest<Result<IEnumerable<FooDto>>>, IProjectRequest
    {
        public GetFoosInProjectQuery(string projectName, bool includeVoided = false)
        {
            ProjectName = projectName;
            IncludeVoided = includeVoided;
        }

        public string ProjectName { get; }
        public bool IncludeVoided { get; }
    }
}
