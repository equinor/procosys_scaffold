using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.ForeignApi.MainApi.Project
{
    public interface IProjectApiService
    {
        Task<ProCoSysProject> TryGetProjectAsync(string plant, string name);
        Task<IList<ProCoSysProject>> GetProjectsInPlantAsync(string plant);
    }
}
