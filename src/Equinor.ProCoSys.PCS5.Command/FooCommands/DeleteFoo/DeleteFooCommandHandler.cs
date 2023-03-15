using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo
{
    public class DeleteFooCommandHandler : IRequestHandler<DeleteFooCommand, Result<string>>
    {
        private readonly IFooRepository _fooRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteFooCommandHandler> _logger;

        public DeleteFooCommandHandler(
            IFooRepository fooRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteFooCommandHandler> logger)
        {
            _fooRepository = fooRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(DeleteFooCommand request, CancellationToken cancellationToken)
        {
            var foo = await _fooRepository.GetByIdAsync(request.FooId);

            foo.SetRowVersion(request.RowVersion);

            // must void before delete.
            // A more real business case is to void the Foo via separate command first, then delete
            foo.IsVoided = true;

            _fooRepository.Remove(foo);

            _logger.LogInformation($"Deleting Foo '{foo.Title}'");

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new SuccessResult<string>(foo.RowVersion.ConvertToString());
        }
    }
}
