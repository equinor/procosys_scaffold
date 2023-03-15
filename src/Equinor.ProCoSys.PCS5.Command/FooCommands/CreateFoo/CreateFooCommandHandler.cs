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
using Equinor.ProCoSys.PCS5.ForeignApi.MainApi.Project;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo
{
    public class CreateFooCommandHandler : IRequestHandler<CreateFooCommand, Result<IdAndRowVersion>>
    {
        private readonly ILogger<CreateFooCommandHandler> _logger;

        private readonly IPlantProvider _plantProvider;
        private readonly IFooRepository _fooRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectApiService _projectApiService;

        public CreateFooCommandHandler(
            IPlantProvider plantProvider,
            IFooRepository fooRepository,
            IUnitOfWork unitOfWork,
            IProjectRepository projectRepository,
            IProjectApiService projectApiService,
            ILogger<CreateFooCommandHandler> logger)
        {
            _plantProvider = plantProvider;
            _fooRepository = fooRepository;
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _projectApiService = projectApiService;
            _logger = logger;
        }

        public async Task<Result<IdAndRowVersion>> Handle(CreateFooCommand request, CancellationToken cancellationToken)
        {
            var project = await GetOrCreateProjectAsync(request);

            var foo = new Foo(_plantProvider.Plant, project, request.Title);
            _fooRepository.Add(foo);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Foo '{request.Title}' created");

            return new SuccessResult<IdAndRowVersion>(new IdAndRowVersion(foo.Id, foo.RowVersion.ConvertToString()));
        }

        private async Task<Project> GetOrCreateProjectAsync(CreateFooCommand request) 
            => await _projectRepository.GetProjectOnlyByNameAsync(request.ProjectName) ?? await AddProjectAsync(request);

        private async Task<Project> AddProjectAsync(CreateFooCommand request)
        {
            var proCoSysProject = await _projectApiService.TryGetProjectAsync(_plantProvider.Plant, request.ProjectName);
            if (proCoSysProject is null)
            {
                throw new Exception(
                    $"Could not find ProCoSys project called {request.ProjectName} in plant {_plantProvider.Plant}");
            }

            var project = new Project(_plantProvider.Plant, request.ProjectName, proCoSysProject.Description);
            project.IsClosed = proCoSysProject.IsClosed;

            _projectRepository.Add(project);
            return project;
        }
    }
}
