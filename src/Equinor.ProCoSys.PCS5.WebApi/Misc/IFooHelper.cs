using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.WebApi.Misc
{
    public interface IFooHelper
    {
        Task<string?> GetProjectNameAsync(int fooId);
    }
}
