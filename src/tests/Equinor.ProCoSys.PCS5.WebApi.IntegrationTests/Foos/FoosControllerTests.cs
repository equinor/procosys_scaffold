using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos;

[TestClass]
public class FoosControllerTests : TestBase
{
    private Guid _fooGuidUnderTest;
    private List<FooDto> _initialFoosInProject;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _fooGuidUnderTest = TestFactory.Instance.SeededData[KnownPlantData.PlantA].FooAGuid;
        _initialFoosInProject = await FoosControllerTestsHelper
            .GetAllFoosInProjectAsync(UserType.Writer, TestFactory.PlantWithAccess, TestFactory.ProjectWithAccess);
    }

    [TestMethod]
    public async Task CreateFoo_AsWriter_ShouldCreateFoo()
    {
        // Arrange
        var title = Guid.NewGuid().ToString();

        // Act
        var guidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            title,
            TestFactory.ProjectWithAccess);

        // Assert
        AssertValidGuidAndRowVersion(guidAndRowVersion);
        var newFoo = await FoosControllerTestsHelper
            .GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, guidAndRowVersion.Guid);
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
            .GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooGuidUnderTest);

        // Assert
        Assert.AreEqual(_fooGuidUnderTest, foo.Guid);
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
        var newText = Guid.NewGuid().ToString();
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooGuidUnderTest);
        var initialRowVersion = foo.RowVersion;

        // Act
        var newRowVersion = await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            foo.Guid,
            newTitle,
            newText,
            initialRowVersion);

        // Assert
        AssertRowVersionChange(initialRowVersion, newRowVersion);
        foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooGuidUnderTest);
        Assert.AreEqual(newTitle, foo.Title);
        Assert.AreEqual(newText, foo.Text);
        Assert.AreEqual(newRowVersion, foo.RowVersion);
    }

    [TestMethod]
    public async Task VoidFoo_AsWriter_ShouldVoidFoo()
    {
        // Arrange
        var guidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);
        var initialRowVersion = guidAndRowVersion.RowVersion;

        // Act
        var newRowVersion = await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            guidAndRowVersion.Guid,
            guidAndRowVersion.RowVersion);

        // Assert
        AssertRowVersionChange(initialRowVersion, newRowVersion);
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, guidAndRowVersion.Guid);
        Assert.IsTrue(foo.IsVoided);
        Assert.AreEqual(newRowVersion, foo.RowVersion);
    }

    [TestMethod]
    public async Task DeleteFoo_AsWriter_ShouldDeleteFoo()
    {
        // Arrange
        var guidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);
        var newRowVersion = await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            guidAndRowVersion.Guid,
            guidAndRowVersion.RowVersion);
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, guidAndRowVersion.Guid);
        Assert.IsNotNull(foo);

        // Act
        await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.Writer, TestFactory.PlantWithAccess,
            guidAndRowVersion.Guid,
            newRowVersion);

        // Assert
        await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, guidAndRowVersion.Guid, HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task CreateFooLink_AsWriter_ShouldCreateFooLink()
    {
        // Arrange and Act
        var (_, linkGuidAndRowVersion)
            = await CreateFooLinkAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        // Assert
        AssertValidGuidAndRowVersion(linkGuidAndRowVersion);
    }

    [TestMethod]
    public async Task GetFooLinksAsync_AsWriter_ShouldGetFooLinks()
    {
        // Arrange and Act
        var title = Guid.NewGuid().ToString();
        var url = Guid.NewGuid().ToString();
        var (fooGuidAndRowVersion, linkGuidAndRowVersion) = await CreateFooLinkAsync(title, url);

        // Act
        var links = await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);

        // Assert
        AssertFirstLink(
            fooGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.RowVersion,
            title,
            url,
            links);
    }

    [TestMethod]
    public async Task UpdateFooLink_AsWriter_ShouldUpdateFooLinkAndRowVersion()
    {
        // Arrange
        var newTitle = Guid.NewGuid().ToString();
        var newUrl = Guid.NewGuid().ToString();
        var (fooGuidAndRowVersion, linkGuidAndRowVersion) =
            await CreateFooLinkAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        // Act
        var newRowVersion = await FoosControllerTestsHelper.UpdateFooLinkAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.Guid,
            newTitle,
            newUrl,
            linkGuidAndRowVersion.RowVersion);

        // Assert
        var links = await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);

        AssertRowVersionChange(linkGuidAndRowVersion.RowVersion, newRowVersion);
        AssertFirstLink(
            fooGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.Guid,
            newRowVersion,
            newTitle, 
            newUrl,
            links);
    }

    [TestMethod]
    public async Task DeleteFooLink_AsWriter_ShouldDeleteFooLink()
    {
        // Arrange
        var (fooGuidAndRowVersion, linkGuidAndRowVersion)
            = await CreateFooLinkAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        // Act
        await FoosControllerTestsHelper.DeleteFooLinkAsync(
            UserType.Writer, TestFactory.PlantWithAccess,
            idAndRowVersion.Guid,
            newRowVersion);

        // Assert
        await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, idAndRowVersion.Guid, HttpStatusCode.NotFound);
    }

    private async Task<(GuidAndRowVersion fooGuidAndRowVersion, GuidAndRowVersion linkGuidAndRowVersion)> CreateFooLinkAsync(
        string title,
        string url)
    {
        var fooGuidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);

        var linkGuidAndRowVersion = await FoosControllerTestsHelper.CreateFooLinkAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            title,
            url);

        return (fooGuidAndRowVersion, linkGuidAndRowVersion);
    }

    private static void AssertFirstLink(
        Guid fooGuid,
        Guid linkGuid,
        string linkRowVersion,
        string title,
        string url,
        List<LinkDto> links)
    {
        Assert.IsNotNull(links);
        Assert.AreEqual(1, links.Count);
        var newLink = links[0];
        Assert.AreEqual(fooGuid, newLink.SourceGuid);
        Assert.AreEqual(linkGuid, newLink.Guid);
        Assert.AreEqual(linkRowVersion, newLink.RowVersion);
        Assert.AreEqual(title, newLink.Title);
        Assert.AreEqual(url, newLink.Url);
    }
}
