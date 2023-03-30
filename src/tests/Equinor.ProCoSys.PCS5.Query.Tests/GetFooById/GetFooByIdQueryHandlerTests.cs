using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Query.GetFooById;

namespace Equinor.ProCoSys.PCS5.Query.Tests.GetFooById;

[TestClass]
public class GetFooByIdQueryHandlerTests : ReadOnlyTestsBase
{
    private Foo _foo;
    private int _fooId;

    protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
    {
        using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        _foo = new Foo(TestPlantA, _projectA, "Title");

        context.Foos.Add(_foo);
        context.SaveChangesAsync().Wait();
        _fooId = _foo.Id;
    }

    [TestMethod]
    public async Task Handler_ShouldReturnNotFound_IfFooIsNotFound()
    {
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        var query = new GetFooByIdQuery(500);
        var dut = new GetFooByIdQueryHandler(context);

        var result = await dut.Handle(query, default);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResultType.NotFound, result.ResultType);
        Assert.IsNull(result.Data);
    }

    [TestMethod]
    public async Task Handler_ShouldReturnCorrectFoo()
    {
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
            
        var query = new GetFooByIdQuery(_fooId);
        var dut = new GetFooByIdQueryHandler(context);

        var result = await dut.Handle(query, default);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResultType.Ok, result.ResultType);

        AssertFoo(result.Data, _foo);
    }

    private void AssertFoo(FooDetailsDto fooDetailsDto, Foo foo)
    {
        Assert.AreEqual(foo.Title, fooDetailsDto.Title);
        Assert.IsFalse(foo.IsVoided);
        var project = GetProjectById(foo.ProjectId);
        Assert.AreEqual(project.Name, fooDetailsDto.ProjectName);

        var createdBy = fooDetailsDto.CreatedBy;
        Assert.IsNotNull(createdBy);
        Assert.AreEqual(CurrentUserOid, createdBy.AzureOid);
    }
}