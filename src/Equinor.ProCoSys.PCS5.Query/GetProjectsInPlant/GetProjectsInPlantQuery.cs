using System.Collections.Generic;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.GetProjectsInPlant;

public class GetProjectsInPlantQuery : IRequest<Result<List<ProCoSysProjectDto>>>
{
}