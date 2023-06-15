using System;
using System.Collections.Generic;
using System.Linq;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure.Repositories;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Tests.Repositories;

[TestClass]
public class FooRepositoryTests : EntityWithGuidRepositoryTestBase<Foo>
{
    private Project _project;

    protected override void SetupRepositoryWithOneKnownItem()
    {
        _project = new Project(TestPlant, Guid.NewGuid(), "ProjectName", "Description of project");
        var foo = new Foo(TestPlant, _project, "Foo X");
        _knownGuid = foo.Guid;
        foo.SetProtectedIdForTesting(_knownId);

        var foos = new List<Foo> { foo };

        _dbSetMock = foos.AsQueryable().BuildMockDbSet();

        _contextHelper
            .ContextMock
            .Setup(x => x.Foos)
            .Returns(_dbSetMock.Object);

        _dut = new FooRepository(_contextHelper.ContextMock.Object);
    }

    protected override Foo GetNewEntity() => new(TestPlant, _project, "New Foo");
}
