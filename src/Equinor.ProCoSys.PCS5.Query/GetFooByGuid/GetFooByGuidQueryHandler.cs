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

namespace Equinor.ProCoSys.PCS5.Query.GetFooByGuid;

public class GetFooByGuidQueryHandler : IRequestHandler<GetFooByGuidQuery, Result<FooDetailsDto>>
{
    private readonly IReadOnlyContext _context;

    public GetFooByGuidQueryHandler(IReadOnlyContext context) => _context = context;

    public async Task<Result<FooDetailsDto>> Handle(GetFooByGuidQuery request, CancellationToken cancellationToken)
    {
        var fooDto =
            await (from foo in _context.QuerySet<Foo>()
                    join pro in _context.QuerySet<Project>()
                        on EF.Property<int>(foo, "ProjectId") equals pro.Id
                    join per in _context.QuerySet<Person>()
                        on EF.Property<int>(foo, "CreatedById") equals per.Id
                    where foo.Guid == request.FooGuid
                    select new FooDetailsDto(
                        foo.Guid,
                        pro.Name,
                        foo.Title,
                        foo.Text,
                        new PersonDto(per.Id, per.FirstName, per.LastName, per.UserName, per.Oid, per.Email),
                        foo.IsVoided,
                        foo.RowVersion.ConvertToString())
                )
                .TagWith($"{nameof(GetFooByGuidQueryHandler)}: foo")
                .SingleOrDefaultAsync(cancellationToken);

        if (fooDto == null)
        {
            return new NotFoundResult<FooDetailsDto>(Strings.EntityNotFound(nameof(Foo), request.FooGuid));
        }

        return new SuccessResult<FooDetailsDto>(fooDto);
    }
}
