using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using MediatR;
using ServiceResult;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;

public class CreateFooCommandHandler : IRequestHandler<CreateFooCommand, Result<GuidAndRowVersion>>
{
    private readonly ILogger<CreateFooCommandHandler> _logger;

    private readonly IPlantProvider _plantProvider;
    private readonly IFooRepository _fooRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;

    public CreateFooCommandHandler(
        IPlantProvider plantProvider,
        IFooRepository fooRepository,
        IUnitOfWork unitOfWork,
        IProjectRepository projectRepository,
        ILogger<CreateFooCommandHandler> logger)
    {
        _plantProvider = plantProvider;
        _fooRepository = fooRepository;
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
        _logger = logger;
    }

    public async Task<Result<GuidAndRowVersion>> Handle(CreateFooCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.TryGetProjectByNameAsync(request.ProjectName);
        if (project is null)
        {
            throw new Exception($"Could not find ProCoSys project called {request.ProjectName} in plant {_plantProvider.Plant}");
        }

        var foo = new Foo(_plantProvider.Plant, project, request.Title);
        _fooRepository.Add(foo);
        foo.AddDomainEvent(new FooCreatedEvent(foo));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Foo '{request.Title}' created");

        return new SuccessResult<GuidAndRowVersion>(new GuidAndRowVersion(foo.Guid, foo.RowVersion.ConvertToString()));
    }
}
