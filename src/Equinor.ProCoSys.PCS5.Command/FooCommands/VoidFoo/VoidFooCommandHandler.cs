using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.VoidFoo
{
    // todo add unit tests
    public class VoidFooCommandHandler : IRequestHandler<VoidFooCommand, Result<string>>
    {
        private readonly IFooRepository _fooRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VoidFooCommandHandler> _logger;

        public VoidFooCommandHandler(
            IFooRepository fooRepository,
            IUnitOfWork unitOfWork,
            ILogger<VoidFooCommandHandler> logger)
        {
            _fooRepository = fooRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(VoidFooCommand request, CancellationToken cancellationToken)
        {
            var foo = await _fooRepository.GetByIdAsync(request.FooId);

            foo.IsVoided = true;
            foo.SetRowVersion(request.RowVersion);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation($"Foo '{foo.Title}' voided");
            
            return new SuccessResult<string>(foo.RowVersion.ConvertToString());
        }
    }
}
