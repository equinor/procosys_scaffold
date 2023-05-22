using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure.Repositories;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Tests.Repositories;

[TestClass]
public class FooRepositoryTests : RepositoryTestBase
{
    private readonly int _fooGuid = 5;
    private Foo _foo;
    private Mock<DbSet<Foo>> _fooSetMock;
    private FooRepository _dut;

    [TestInitialize]
    public void Setup()
    {
        var project = new Project(TestPlant, Guid.NewGuid(), "ProjectName", "Description of project");
        _foo = new Foo(TestPlant, project, "Foo X");
        _foo.SetProtectedIdForTesting(_fooGuid);

        var foos = new List<Foo> { _foo };

        _fooSetMock = foos.AsQueryable().BuildMockDbSet();

        ContextHelper
            .ContextMock
            .Setup(x => x.Foos)
            .Returns(_fooSetMock.Object);

        _dut = new FooRepository(ContextHelper.ContextMock.Object);
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnAllItems()
    {
        var result = await _dut.GetAllAsync();

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public async Task GetByIds_UnknownId_ShouldReturnEmptyList()
    {
        var result = await _dut.GetByIdsAsync(new List<int> { 1234 });

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task Exists_KnownId_ShouldReturnTrue()
    {
        var result = await _dut.Exists(_fooGuid);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task Exists_UnknownId_ShouldReturnFalse()
    {
        var result = await _dut.Exists(1234);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task GetById_KnownId_ShouldReturnFoo()
    {
        var result = await _dut.GetByIdAsync(_fooGuid);

        Assert.IsNotNull(result);
        Assert.AreEqual(_fooGuid, result.Id);
    }

    [TestMethod]
    public async Task GetById_UnknownId_ShouldReturnNull()
    {
        var result = await _dut.GetByIdAsync(1234);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Add_Foo_ShouldCallAdd()
    {
        _dut.Add(_foo);

        _fooSetMock.Verify(x => x.Add(_foo), Times.Once);
    }
}