using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoo;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.Tests.GetFoo;

[TestClass]
public class GetFooByGuidQueryHandlerTests : ReadOnlyTestsBase
{
    private Foo _createdFoo;
    private Guid _createdFooGuid;
    private Foo _modifiedFoo;
    private Guid _modifiedFooGuid;

    protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
    {
        using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        _createdFoo = new Foo(TestPlantA, _projectA, "TitleA");
        _modifiedFoo = new Foo(TestPlantA, _projectA, "TitleB");

        context.Foos.Add(_createdFoo);
        context.Foos.Add(_modifiedFoo);
        context.SaveChangesAsync().Wait();
        _createdFooGuid = _createdFoo.Guid;

        _modifiedFoo.EditFoo("TitleB modified", "Modified");
        context.SaveChangesAsync().Wait();
        _modifiedFooGuid = _modifiedFoo.Guid;
    }

    [TestMethod]
    public async Task Handler_ShouldReturnNotFound_IfFooIsNotFound()
    {
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        var query = new GetFooQuery(Guid.Empty);
        var dut = new GetFooQueryHandler(context);

        var result = await dut.Handle(query, default);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResultType.NotFound, result.ResultType);
        Assert.IsNull(result.Data);
    }

    [TestMethod]
    public async Task Handler_ShouldReturnCorrectCreatedFoo()
    {
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
            
        var query = new GetFooQuery(_createdFooGuid);
        var dut = new GetFooQueryHandler(context);

        var result = await dut.Handle(query, default);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResultType.Ok, result.ResultType);

        var fooDetailsDto = result.Data;
        AssertFoo(fooDetailsDto, _createdFoo);
        Assert.IsNull(fooDetailsDto.ModifiedBy);
        Assert.IsNull(fooDetailsDto.ModifiedAtUtc);
    }

    [TestMethod]
    public async Task Handler_ShouldReturnCorrectModifiedFoo()
    {
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        var query = new GetFooQuery(_modifiedFooGuid);
        var dut = new GetFooQueryHandler(context);

        var result = await dut.Handle(query, default);

        Assert.IsNotNull(result);
        Assert.AreEqual(ResultType.Ok, result.ResultType);

        var fooDetailsDto = result.Data;
        AssertFoo(fooDetailsDto, _modifiedFoo);
        var modifiedBy = fooDetailsDto.ModifiedBy;
        Assert.IsNotNull(modifiedBy);
        Assert.AreEqual(CurrentUserOid, modifiedBy.AzureOid);
        Assert.IsNotNull(fooDetailsDto.ModifiedAtUtc);
        Assert.AreEqual(_modifiedFoo.ModifiedAtUtc, fooDetailsDto.ModifiedAtUtc);
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
        Assert.AreEqual(foo.CreatedAtUtc, fooDetailsDto.CreatedAtUtc);
    }
}
