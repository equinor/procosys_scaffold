using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Microsoft.EntityFrameworkCore;
using Equinor.ProCoSys.Common;
using System;

namespace Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;

public class FooValidator : IFooValidator
{
    private readonly IReadOnlyContext _context;

    public FooValidator(IReadOnlyContext context) => _context = context;

    public async Task<bool> FooExistsAsync(Guid fooGuid, CancellationToken cancellationToken) =>
        await (from foo in _context.QuerySet<Foo>()
            where foo.Guid == fooGuid
            select foo).AnyAsync(cancellationToken);

    public async Task<bool> FooIsVoidedAsync(Guid fooGuid, CancellationToken cancellationToken)
    {
        var foo = await (from f in _context.QuerySet<Foo>()
            where f.Guid == fooGuid
            select f).SingleOrDefaultAsync(cancellationToken);
        return foo != null && foo.IsVoided;

    }
}
