using System;
using System.Threading;
using System.Threading.Tasks;

namespace Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;

public interface IFooValidator
{
    Task<bool> FooExistsAsync(Guid fooGuid, CancellationToken cancellationToken);
    Task<bool> FooIsVoidedAsync(Guid fooGuid, CancellationToken cancellationToken);
}
