using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Repositories
{
    public class FooRepository : RepositoryBase<Foo>, IFooRepository
    {
        public FooRepository(PCS5Context context)
            : base(context, context.Foos)
        {
        }
    }
}
