using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.FooEvents;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo;

public class DeleteFooCommandHandler : IRequestHandler<DeleteFooCommand, Result<Unit>>
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

    public async Task<Result<Unit>> Handle(DeleteFooCommand request, CancellationToken cancellationToken)
    {
        var foo = await _fooRepository.GetByGuidAsync(request.FooGuid);
        if (foo == null)
        {
            throw new Exception($"Entity {nameof(Foo)} {request.FooGuid} not found");
        }

        foo.SetRowVersion(request.RowVersion);
        _fooRepository.Remove(foo);
        foo.AddDomainEvent(new FooDeletedEvent(foo));

        _logger.LogInformation($"Deleting Foo '{foo.Title}'");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
            
        return new SuccessResult<Unit>(Unit.Value);
    }
}
