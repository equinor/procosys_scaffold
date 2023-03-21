using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Microsoft.EntityFrameworkCore;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.Command.Validators.FooValidators
{
    public class FooValidator : IFooValidator
    {
        private readonly IReadOnlyContext _context;

        public FooValidator(IReadOnlyContext context) => _context = context;

        public async Task<bool> FooExistsAsync(int fooId, CancellationToken cancellationToken) =>
            await (from foo in _context.QuerySet<Foo>()
                   where foo.Id == fooId
                   select foo).AnyAsync(cancellationToken);

        public async Task<bool> FooIsVoidedAsync(int fooId, CancellationToken cancellationToken)
        {
            var foo = await (from f in _context.QuerySet<Foo>()
                where f.Id == fooId
                select f).SingleOrDefaultAsync(cancellationToken);
            return foo != null && foo.IsVoided;

        }

        public bool FooIsOk()
        {
            // some business logic for foo here
            return true;
        }
    }
}
