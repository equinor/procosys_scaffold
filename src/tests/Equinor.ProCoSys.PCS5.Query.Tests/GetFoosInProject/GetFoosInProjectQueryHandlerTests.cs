using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoosInProject;

namespace Equinor.ProCoSys.PCS5.Query.Tests.GetFoosInProject;

[TestClass]
public class GetFoosInProjectQueryHandlerTests : ReadOnlyTestsBase
{
    private Foo _nonVoidedFooInProjectA;
    private Foo _voidedFooInProjectA;
    private Foo _fooInProjectB;

    protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
    {
        using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        _nonVoidedFooInProjectA = new Foo(TestPlantA, _projectA, "NonVoidedA");
        _voidedFooInProjectA = new Foo(TestPlantA, _projectA, "VoidedA") { IsVoided = true };
        _fooInProjectB = new Foo(TestPlantA, _projectB, "B");

        context.Foos.Add(_nonVoidedFooInProjectA);
        context.Foos.Add(_voidedFooInProjectA);
        context.Foos.Add(_fooInProjectB);
        context.SaveChangesAsync().Wait();
    }

    [TestMethod]
    public async Task Handler_ShouldReturnEmptyList_IfNoneFound()
    {
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        var query = new GetFoosInProjectQuery("UnknownProject");
        var dut = new GetFoosInProjectQueryHandler(context);

        var result = await dut.Handle(query, default);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResultType.Ok, result.ResultType);
        Assert.AreEqual(0, result.Data.Count());
    }

    [TestMethod]
    public async Task Handler_ShouldReturnCorrectFoos()
    {
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        var query = new GetFoosInProjectQuery(_projectA.Name, true);
        var dut = new GetFoosInProjectQueryHandler(context);

        var result = await dut.Handle(query, default);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResultType.Ok, result.ResultType);
        Assert.AreEqual(2, result.Data.Count());

        AssertFoo(result.Data.Single(f => !f.IsVoided), _nonVoidedFooInProjectA);
        AssertFoo(result.Data.Single(f => f.IsVoided), _voidedFooInProjectA);
    }

    [TestMethod]
    public async Task Handler_ShouldReturnNonVoidedFoos()
    {
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        var query = new GetFoosInProjectQuery(_projectA.Name);
        var dut = new GetFoosInProjectQueryHandler(context);

        var result = await dut.Handle(query, default);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResultType.Ok, result.ResultType);
        Assert.AreEqual(1, result.Data.Count());

        AssertFoo(result.Data.Single(f => !f.IsVoided), _nonVoidedFooInProjectA);
    }

    private void AssertFoo(FooDto fooDto, Foo foo)
    {
        Assert.AreEqual(foo.Title, fooDto.Title);
        var project = GetProjectById(foo.ProjectId);
        Assert.AreEqual(project.Name, fooDto.ProjectName);
    }
}
