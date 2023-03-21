using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.GetFooById
{
    public class GetFooByIdQueryHandler : IRequestHandler<GetFooByIdQuery, Result<FooDetailsDto>>
    {
        private readonly IReadOnlyContext _context;

        public GetFooByIdQueryHandler(IReadOnlyContext context) => _context = context;

        public async Task<Result<FooDetailsDto>> Handle(GetFooByIdQuery request, CancellationToken cancellationToken)
        {
            var fooDto =
                await (from foo in _context.QuerySet<Foo>()
                        join pro in _context.QuerySet<Project>()
                            on EF.Property<int>(foo, "ProjectId") equals pro.Id
                        join per in _context.QuerySet<Person>()
                            on EF.Property<int>(foo, "CreatedById") equals per.Id
                            where foo.Id == request.FooId
                       select new FooDetailsDto(
                            foo.Id,
                            pro.Name,
                            foo.Title,
                            new PersonDto(per.Id, per.FirstName, per.LastName, per.UserName, per.Oid, per.Email),
                            foo.IsVoided,
                            foo.RowVersion.ConvertToString())
                    )
                    .TagWith($"{nameof(GetFooByIdQueryHandler)}: foo")
                    .SingleOrDefaultAsync(cancellationToken);

            if (fooDto == null)
            {
                return new NotFoundResult<FooDetailsDto>(Strings.EntityNotFound(nameof(Foo), request.FooId));
            }

            return new SuccessResult<FooDetailsDto>(fooDto);
        }
    }
}
