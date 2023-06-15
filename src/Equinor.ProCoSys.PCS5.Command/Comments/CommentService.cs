using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.CommentAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.CommentEvents;
using Microsoft.Extensions.Logging;

namespace Equinor.ProCoSys.PCS5.Command.Comments;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CommentService> _logger;

    public CommentService(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        ILogger<CommentService> logger)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CommentDto> AddAsync(
        string sourceType,
        Guid sourceGuid,
        string text,
        CancellationToken cancellationToken)
    {
        var comment = new Comment(sourceType, sourceGuid, text);
        _commentRepository.Add(comment);
        comment.AddDomainEvent(new CommentCreatedEvent(comment));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogDebug($"Comment with guid {comment.Guid} created for {comment.SourceGuid}");

        return new CommentDto(comment.Guid, comment.RowVersion.ConvertToString());
    }
}
