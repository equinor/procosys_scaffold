using System.Collections.Generic;
using Equinor.ProCoSys.Common;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.GetFoosInProject
{
    public class GetFoosInProjectQuery : IRequest<Result<IEnumerable<FooDto>>>, IProjectRequest
    {
        public GetFoosInProjectQuery(string projectName) => ProjectName = projectName;

        public string ProjectName { get; }
    }
}
