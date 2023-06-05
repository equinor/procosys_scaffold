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

namespace Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoo;

public class GetFooQueryHandler : IRequestHandler<GetFooQuery, Result<FooDetailsDto>>
{
    private readonly IReadOnlyContext _context;

    public GetFooQueryHandler(IReadOnlyContext context) => _context = context;

    public async Task<Result<FooDetailsDto>> Handle(GetFooQuery request, CancellationToken cancellationToken)
    {
        var dto =
            await (from foo in _context.QuerySet<Foo>()
                   join project in _context.QuerySet<Project>()
                       on EF.Property<int>(foo, "ProjectId") equals project.Id
                   join createdByUser in _context.QuerySet<Person>()
                       on EF.Property<int>(foo, "CreatedById") equals createdByUser.Id
                   from modifiedByUser in _context.QuerySet<Person>()
                       .Where(p => p.Id == EF.Property<int>(foo, "ModifiedById")).DefaultIfEmpty() //left join!
                   where foo.Guid == request.FooGuid
                   select new {
                       Foo = foo,
                       Project = project,
                       CreatedByUser = createdByUser, 
                       ModifiedByUser = modifiedByUser
                })
                .TagWith($"{nameof(GetFooQueryHandler)}.{nameof(Handle)}")
                .SingleOrDefaultAsync(cancellationToken);

        if (dto == null)
        {
            return new NotFoundResult<FooDetailsDto>(Strings.EntityNotFound(nameof(Foo), request.FooGuid));
        }

        var createdBy = new PersonDto(
            dto.CreatedByUser.Id,
            dto.CreatedByUser.FirstName,
            dto.CreatedByUser.LastName,
            dto.CreatedByUser.UserName,
            dto.CreatedByUser.Guid,
            dto.CreatedByUser.Email);
        
        PersonDto? modifiedBy = null;
        if (dto.ModifiedByUser != null)
        {
            modifiedBy = new PersonDto(
                dto.ModifiedByUser.Id,
                dto.ModifiedByUser.FirstName,
                dto.ModifiedByUser.LastName,
                dto.ModifiedByUser.UserName,
                dto.ModifiedByUser.Guid,
                dto.ModifiedByUser.Email);
        }

        var fooDto = new FooDetailsDto(
                       dto.Foo.Guid,
                       dto.Project.Name,
                       dto.Foo.Title,
                       dto.Foo.Text,
                       createdBy,
                       dto.Foo.CreatedAtUtc,
                       modifiedBy,
                       dto.Foo.ModifiedAtUtc,
                       dto.Foo.IsVoided,
                       dto.Foo.RowVersion.ConvertToString());
        return new SuccessResult<FooDetailsDto>(fooDto);
    }
}
