using Equinor.ProCoSys.PCS5.Application.Dtos;
using Equinor.ProCoSys.PCS5.Application.Services;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.LinkEvents;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.Extensions.Logging;
using Moq;

namespace Equinor.ProCoSys.PCS5.Application.Tests.Services;

[TestClass]
public class LinkServiceTests : TestsBase
{
    private readonly Guid _sourceGuid = Guid.NewGuid();
    private Mock<ILinkRepository> _linkRepositoryMock;
    private LinkService _dut;
    private Link _linkAddedToRepository;
    private Link _link;
    private List<Link> _links;

    [TestInitialize]
    public void Setup()
    {
        _linkRepositoryMock = new Mock<ILinkRepository>();
        _linkRepositoryMock
            .Setup(x => x.Add(It.IsAny<Link>()))
            .Callback<Link>(link =>
            {
                _linkAddedToRepository = link;
            });
        _link = new Link("Whatever", _sourceGuid, "T", "www");
        _links = new List<Link> { _link };
        _linkRepositoryMock.Setup(l => l.GetAllForSourceAsync(_sourceGuid))
            .ReturnsAsync(_links);

        _dut = new LinkService(
            _linkRepositoryMock.Object,
            _unitOfWorkMock.Object,
            new Mock<ILogger<LinkService>>().Object);
    }

    #region AddAsync
    [TestMethod]
    public async Task HandlingAddAsync_ShouldReturn_LinkDto()
    {
        // Act
        var result = await _dut.AddAsync("Whatever", _sourceGuid, "T", "www", default);

        // Assert
        Assert.IsInstanceOfType(result, typeof(LinkDto));
    }

    [TestMethod]
    public async Task HandlingAddAsync_ShouldAddLinkToRepository()
    {
        // Act
        await _dut.AddAsync("Whatever", _sourceGuid, "T", "www", default);

        // Assert
        Assert.IsNotNull(_linkAddedToRepository);
    }

    [TestMethod]
    public async Task HandlingAddAsync_ShouldSaveOnce()
    {
        // Act
        var result = await _dut.AddAsync("Whatever", _sourceGuid, "T", "www", default);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task HandlingAddAsync_ShouldAddLinkCreatedEvent()
    {
        // Act
        var result = await _dut.AddAsync("Whatever", _sourceGuid, "T", "www", default);

        // Assert
        Assert.IsInstanceOfType(_linkAddedToRepository.DomainEvents.First(), typeof(LinkCreatedEvent));
    }
    #endregion

    #region GetAllForSourceAsync
    [TestMethod]
    public async Task HandlingGetAllForSourceAsync_ShouldReturn_LinkDto()
    {
        // Act
        var result = await _dut.GetAllForSourceAsync(_sourceGuid, default);

        // Assert
        Assert.IsInstanceOfType(result, typeof(IEnumerable<LinkDto>));
    }

    [TestMethod]
    public async Task HandlingGetAllForSourceAsync_ShouldReturnCorrectDto()
    {
        // Act
        var result = await _dut.GetAllForSourceAsync(_sourceGuid, default);

        // Assert
        Assert.AreEqual(1, result.Count());
        var linkDto = result.ElementAt(0);
        Assert.AreEqual(_link.Title, linkDto.Title);
        Assert.AreEqual(_link.Url, linkDto.Url);
        Assert.AreEqual(_link.Guid, linkDto.Guid);
    }
    #endregion
}
