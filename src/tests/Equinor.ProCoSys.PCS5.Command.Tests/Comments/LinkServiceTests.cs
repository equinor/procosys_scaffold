using System;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Comments;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.CommentAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.CommentEvents;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.Comments;

[TestClass]
public class CommentServiceTests : TestsBase
{
    private readonly Guid _sourceGuid = Guid.NewGuid();
    private readonly Guid _commentGuid = Guid.NewGuid();
    private Mock<ICommentRepository> _commentRepositoryMock;
    private CommentService _dut;
    private Comment _commentAddedToRepository;
    private Comment _existingComment;

    [TestInitialize]
    public void Setup()
    {
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _commentRepositoryMock
            .Setup(x => x.Add(It.IsAny<Comment>()))
            .Callback<Comment>(comment =>
            {
                _commentAddedToRepository = comment;
            });
        _existingComment = new Comment("Whatever", _sourceGuid, "T");
        _commentRepositoryMock.Setup(l => l.TryGetByGuidAsync(_commentGuid))
            .ReturnsAsync(_existingComment);

        _dut = new CommentService(
            _commentRepositoryMock.Object,
            _unitOfWorkMock.Object,
            new Mock<ILogger<CommentService>>().Object);
    }

    #region AddAsync
    [TestMethod]
    public async Task AddAsync_ShouldAddCommentToRepository()
    {
        // Arrange 
        var sourceType = "Whatever";
        var text = "T";

        // Act
        await _dut.AddAsync(sourceType, _sourceGuid, text, default);

        // Assert
        Assert.IsNotNull(_commentAddedToRepository);
        Assert.AreEqual(_sourceGuid, _commentAddedToRepository.SourceGuid);
        Assert.AreEqual(sourceType, _commentAddedToRepository.SourceType);
        Assert.AreEqual(text, _commentAddedToRepository.Text);
    }

    [TestMethod]
    public async Task AddAsync_ShouldSaveOnce()
    {
        // Act
        await _dut.AddAsync("Whatever", _sourceGuid, "T", default);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task AddAsync_ShouldAddCommentCreatedEvent()
    {
        // Act
        await _dut.AddAsync("Whatever", _sourceGuid, "T", default);

        // Assert
        Assert.IsInstanceOfType(_commentAddedToRepository.DomainEvents.First(), typeof(CommentCreatedEvent));
    }
    #endregion
}
