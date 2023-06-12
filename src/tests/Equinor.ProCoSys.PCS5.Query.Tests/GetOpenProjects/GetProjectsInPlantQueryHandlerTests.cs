using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Query.Projects.GetOpenProjects;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.Tests.GetOpenProjects;

[TestClass]
public class GetOpenProjectsQueryHandlerTests : ReadOnlyTestsBase
{
    private readonly GetOpenProjectsQuery _query = new ();

    protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
    {
        using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        var projectB = context.Projects.Single(p => p.Name == ProjectNameB);
        projectB.IsClosed = true;
        context.SaveChangesAsync().Wait();
    }

    [TestMethod]
    public async Task Handle_ShouldReturnOkResult()
    {
        //Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new GetOpenProjectsQueryHandler(context);

        // Act
        var result = await dut.Handle(_query, default);

        // Assert
        Assert.AreEqual(ResultType.Ok, result.ResultType);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnOpenProject()
    {
        //Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new GetOpenProjectsQueryHandler(context);

        // Act
        var result = await dut.Handle(_query, default);

        // Assert
        Assert.AreEqual(1, result.Data.Count);
        var project = result.Data.Single();

        Assert.AreEqual(_projectA.Id, project.Id);
        Assert.AreEqual(_projectA.Name, project.Name);
        Assert.AreEqual(_projectA.Description, project.Description);
    }
}
