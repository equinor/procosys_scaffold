using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooLinks;
using Equinor.ProCoSys.PCS5.Query.Links;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Query.Tests.GetFooLinks;

[TestClass]
public class GetFooLinksQueryHandlerTests : TestsBase
{
    private GetFooLinksQueryHandler _dut;
    private Mock<ILinkService> _linkServiceMock;
    private GetFooLinksQuery _query;
    private LinkDto _linkDto;

    [TestInitialize]
    public void Setup()
    {
        _query = new GetFooLinksQuery(Guid.NewGuid());

        _linkDto = new LinkDto(_query.FooGuid, Guid.NewGuid(), "T", "U", "R");
        var linkDtos = new List<LinkDto>
        {
            _linkDto
        };
        _linkServiceMock = new Mock<ILinkService>();
        _linkServiceMock.Setup(l => l.GetAllForSourceAsync(_query.FooGuid, default))
            .ReturnsAsync(linkDtos);

        _dut = new GetFooLinksQueryHandler(_linkServiceMock.Object);
    }

    [TestMethod]
    public async Task HandlingQuery_ShouldReturn_Links()
    {
        // Act
        var result = await _dut.Handle(_query, default);

        // Assert
        Assert.IsInstanceOfType(result.Data, typeof(IEnumerable<LinkDto>));
        var link = result.Data.Single();
        Assert.AreEqual(_linkDto.SourceGuid, link.SourceGuid);
        Assert.AreEqual(_linkDto.Guid, link.Guid);
        Assert.AreEqual(_linkDto.Title, link.Title);
        Assert.AreEqual(_linkDto.Url, link.Url);
        Assert.AreEqual(_linkDto.RowVersion, link.RowVersion);
    }

    [TestMethod]
    public async Task HandlingQuery_Should_CallGetAllForSource_OnLinkService()
    {
        // Act
        await _dut.Handle(_query, default);

        // Assert
        _linkServiceMock.Verify(u => u.GetAllForSourceAsync(
            _query.FooGuid,
            default), Times.Exactly(1));
    }
}
