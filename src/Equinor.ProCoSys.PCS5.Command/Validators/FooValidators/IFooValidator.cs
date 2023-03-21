using System.Threading;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Command.Validators.FooValidators
{
    public interface IFooValidator
    {
        bool FooIsOk();
        Task<bool> FooExistsAsync(int fooId, CancellationToken cancellationToken);
        Task<bool> FooIsVoidedAsync(int fooId, CancellationToken cancellationToken);
    }
}
