using System.Collections.Generic;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.Projects.GetOpenProjects;

public class GetOpenProjectsQuery : IRequest<Result<List<ProjectDto>>>
{
}
