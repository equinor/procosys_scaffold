using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.GetFooById
{
    public class GetFooByIdQueryHandler : IRequestHandler<GetFooByIdQuery, Result<FooDto>>
    {
        private readonly IReadOnlyContext _context;
        private readonly ILogger<GetFooByIdQueryHandler> _logger;

        public GetFooByIdQueryHandler(
            IReadOnlyContext context,
            ILogger<GetFooByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<FooDto>> Handle(GetFooByIdQuery request, CancellationToken cancellationToken)
        {
            var foo = await _context.QuerySet<Foo>()
                .SingleOrDefaultAsync(x => x.Id == request.FooId, cancellationToken);

            if (foo == null)
            {
                return new NotFoundResult<FooDto>(Strings.EntityNotFound(nameof(Foo), request.FooId));
            }

            var fooDto = await ConvertToFooDtoAsync(foo);

            return new SuccessResult<FooDto>(fooDto);
        }

        private async Task<FooDto> ConvertToFooDtoAsync(Foo foo)
        {
            var project = await _context.QuerySet<Project>()
                .SingleOrDefaultAsync(x => x.Id == foo.ProjectId);

            var createdBy = await _context.QuerySet<Person>()
                .SingleOrDefaultAsync(p => p.Id == foo.CreatedById);

            PersonDto personDto = null;
            if (createdBy != null)
            {
                personDto = new PersonDto(
                    createdBy.Id,
                    createdBy.FirstName,
                    createdBy.LastName,
                    createdBy.UserName,
                    createdBy.Oid,
                    createdBy.Email);
            }
            else
            {
                _logger.LogWarning($"Cannot find person with id {foo.CreatedById} on foo {foo.Title}");
            }
            if (project is null)
            {
                _logger.LogWarning($"Cannot find project with id {foo.ProjectId} on foo {foo.Title}");
            }

            var result = new FooDto(
                foo.Id,
                project?.Name ?? "Project name not found",
                foo.Title,
                personDto,
                foo.RowVersion.ConvertToString());

            return result;
        }
    }
}
