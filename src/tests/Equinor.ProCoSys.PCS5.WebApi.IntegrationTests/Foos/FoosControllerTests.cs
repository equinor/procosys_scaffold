using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos;

[TestClass]
public class FoosControllerTests : TestBase
{
    private int _fooIdUnderTest;
    private List<FooDto> _initialFoosInProject;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _fooIdUnderTest = TestFactory.Instance.SeededData[KnownPlantData.PlantA].FooAId;
        _initialFoosInProject = await FoosControllerTestsHelper
            .GetAllFoosInProjectAsync(UserType.Writer, TestFactory.PlantWithAccess, TestFactory.ProjectWithAccess);
    }

    [TestMethod]
    public async Task CreateFoo_AsWriter_ShouldCreateFoo()
    {
        // Arrange
        var title = Guid.NewGuid().ToString();

        // Act
        var idAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            title,
            TestFactory.ProjectWithAccess);

        // Assert
        Assert.IsNotNull(idAndRowVersion);
        IsAValidRowVersion(idAndRowVersion.RowVersion);
        Assert.IsTrue(idAndRowVersion.Id > 0);
        var newFoo = await FoosControllerTestsHelper
            .GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, idAndRowVersion.Id);
        Assert.IsNotNull(newFoo);
        Assert.AreEqual(title, newFoo.Title);
        Assert.AreEqual(TestFactory.ProjectWithAccess, newFoo.ProjectName);
        AssertCreatedBy(UserType.Writer, newFoo.CreatedBy);

        var allFoos = await FoosControllerTestsHelper
            .GetAllFoosInProjectAsync(UserType.Writer, TestFactory.PlantWithAccess, TestFactory.ProjectWithAccess);
        Assert.AreEqual(_initialFoosInProject.Count+1, allFoos.Count);
    }

    [TestMethod]
    public async Task GetFoo_AsWriter_ShouldGetFoo()
    {
        // Act
        var foo = await FoosControllerTestsHelper
            .GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooIdUnderTest);

        // Assert
        Assert.AreEqual(_fooIdUnderTest, foo.Id);
        Assert.IsNotNull(foo.RowVersion);
    }

    [TestMethod]
    public async Task GetAllFoos_AsWriter_ShouldGetAllFoos()
    {
        // Act
        var foos = await FoosControllerTestsHelper
            .GetAllFoosInProjectAsync(UserType.Writer, TestFactory.PlantWithAccess, TestFactory.ProjectWithAccess);

        // Assert
        Assert.IsTrue(foos.Count > 0);
        Assert.IsTrue(foos.All(f => f.ProjectName == TestFactory.ProjectWithAccess));
        Assert.IsTrue(foos.All(f => !f.Title.IsEmpty()));
        Assert.IsTrue(foos.All(f => !f.RowVersion.IsEmpty()));
    }

    [TestMethod]
    public async Task UpdateFoo_AsWriter_ShouldUpdateFooAndRowVersion()
    {
        // Arrange
        var newTitle = Guid.NewGuid().ToString();
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooIdUnderTest);
        var initialRowVersion = foo.RowVersion;

        // Act
        var newRowVersion = await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            foo.Id,
            newTitle,
            initialRowVersion);

        // Assert
        AssertRowVersionChange(initialRowVersion, newRowVersion);
        foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooIdUnderTest);
        Assert.AreEqual(newTitle, foo.Title);
        Assert.AreEqual(newRowVersion, foo.RowVersion);
    }

    [TestMethod]
    public async Task VoidFoo_AsWriter_ShouldVoidFoo()
    {
        // Arrange
        var idAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);
        var initialRowVersion = idAndRowVersion.RowVersion;

        // Act
        var newRowVersion = await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            idAndRowVersion.Id,
            idAndRowVersion.RowVersion);

        // Assert
        AssertRowVersionChange(initialRowVersion, newRowVersion);
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, idAndRowVersion.Id);
        Assert.IsTrue(foo.IsVoided);
        Assert.AreEqual(newRowVersion, foo.RowVersion);
    }

    [TestMethod]
    public async Task DeleteFoo_AsWriter_ShouldDeleteFoo()
    {
        // Arrange
        var idAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);
        var newRowVersion = await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            idAndRowVersion.Id,
            idAndRowVersion.RowVersion);
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, idAndRowVersion.Id);
        Assert.IsNotNull(foo);

        // Act
        await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.Writer, TestFactory.PlantWithAccess,
            idAndRowVersion.Id,
            newRowVersion);

        // Assert
        await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, idAndRowVersion.Id, HttpStatusCode.NotFound);
    }
}