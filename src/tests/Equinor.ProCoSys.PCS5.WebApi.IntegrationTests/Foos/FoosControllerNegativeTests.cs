using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos;

[TestClass]
public class FoosControllerNegativeTests : TestBase
{
    private Guid _fooGuidUnderTest;
    private Guid _linkGuidUnderTest;

    [TestInitialize]
    public void TestInitialize()
    {
        _fooGuidUnderTest = TestFactory.Instance.SeededData[KnownPlantData.PlantA].FooAGuid;
        _linkGuidUnderTest = TestFactory.Instance.SeededData[KnownPlantData.PlantA].LinkInFooAGuid;
    }

    #region GetFoo
    [TestMethod]
    public async Task GetFoo_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.GetFooAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task GetFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.GetFooAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task GetFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.GetFooAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest, 
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task GetFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.GetFooAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest, 
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task GetFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.GetFooAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess, 
            _fooGuidUnderTest, 
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task GetFoo_AsWriter_ShouldReturnNotFound_WhenUnknownFoo()
        => await FoosControllerTestsHelper.GetFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess, 
            Guid.Empty, 
            HttpStatusCode.NotFound);
    #endregion

    #region GetFoosInProject
    [TestMethod]
    public async Task GetFoosInProject_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.GetAllFoosInProjectAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            TestFactory.Unknown,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task GetFoosInProject_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.GetAllFoosInProjectAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            TestFactory.Unknown,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task GetFoosInProject_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.GetAllFoosInProjectAsync(
            UserType.Writer,
            TestFactory.Unknown,
            TestFactory.Unknown,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task GetFoosInProject_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.GetAllFoosInProjectAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            TestFactory.ProjectWithoutAccess,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task GetFoosInProject_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.GetAllFoosInProjectAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            TestFactory.ProjectWithoutAccess,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task GetFoosInProject_AsWriter_ShouldReturnForbidden_WhenNoAccessToProject()
        => await FoosControllerTestsHelper.GetAllFoosInProjectAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            TestFactory.ProjectWithoutAccess,
            HttpStatusCode.Forbidden);
    #endregion

    #region CreateFoo
    [TestMethod]
    public async Task CreateFoo_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            "Foo1",
            "Pro",
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task CreateFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.CreateFooAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            "Foo1",
            "Pro",
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task CreateFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.Unknown,
            "Foo1",
            "Pro",
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task CreateFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.CreateFooAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            "Foo1",
            "Pro",
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task CreateFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            "Foo1", 
            "Pro",
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task CreateFoo_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
        => await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            "Foo1",
            "Pro",
            HttpStatusCode.Forbidden);
    #endregion
        
    #region VoidFoo
    [TestMethod]
    public async Task VoidFoo_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Anonymous, 
            TestFactory.Unknown,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task VoidFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.VoidFooAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task VoidFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task VoidFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.VoidFooAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task VoidFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task VoidFoo_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
        => await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task VoidFoo_AsWriter_ShouldReturnConflict_WhenWrongRowVersion()
        => await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            TestFactory.WrongButValidRowVersion,
            HttpStatusCode.Conflict);

    #endregion

    #region UpdateFoo
    [TestMethod]
    public async Task UpdateFoo_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "Foo1",
            "Foo1-txt",
            TestFactory.AValidRowVersion,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task UpdateFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "Foo1",
            "Foo1-txt",
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task UpdateFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "Foo1",
            "Foo1-txt",
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task UpdateFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            "Foo1",
            "Foo1-txt",
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task UpdateFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            "Foo1",
            "Foo1-txt",
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task UpdateFoo_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
        => await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            "Foo1",
            "Foo1-txt",
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task UpdateFoo_AsWriter_ShouldReturnConflict_WhenWrongRowVersion()
        => await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            TestFactory.WrongButValidRowVersion,
            HttpStatusCode.Conflict);

    #endregion

    #region DeleteFoo
    [TestMethod]
    public async Task DeleteFoo_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task DeleteFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.NoPermissionUser, TestFactory.Unknown,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task DeleteFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task DeleteFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task DeleteFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task DeleteFoo_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
        => await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task DeleteFoo_AsWriter_ShouldReturnConflict_WhenWrongRowVersion()
    {
        var idAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);
        await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            idAndRowVersion.Guid,
            idAndRowVersion.RowVersion);
        // Act

        await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            idAndRowVersion.Guid,
            TestFactory.WrongButValidRowVersion,
            HttpStatusCode.Conflict);
    }
    #endregion

    #region CreateFooLink
    [TestMethod]
    public async Task CreateFooLink_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.CreateFooLinkAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "T",
            "U",
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task CreateFooLink_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.CreateFooLinkAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "T",
            "U",
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task CreateFooLink_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.CreateFooLinkAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "T",
            "U",
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task CreateFooLink_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.CreateFooLinkAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            "T",
            "U",
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task CreateFooLink_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.CreateFooLinkAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            "T",
            "U",
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task CreateFooLink_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
        => await FoosControllerTestsHelper.CreateFooLinkAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            "T",
            "U",
            HttpStatusCode.Forbidden);
    #endregion

    #region GetFooLinks
    [TestMethod]
    public async Task GetFooLinks_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task GetFooLinks_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task GetFooLinks_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task GetFooLinks_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task GetFooLinks_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            HttpStatusCode.Forbidden);
    #endregion

    #region UpdateFooLink
    [TestMethod]
    public async Task UpdateFooLink_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.UpdateFooLinkAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            "T",
            "U",
            TestFactory.AValidRowVersion,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task UpdateFooLink_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.UpdateFooLinkAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            "T",
            "U",
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task UpdateFooLink_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.UpdateFooLinkAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            "T",
            "U",
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task UpdateFooLink_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.UpdateFooLinkAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            "T",
            "U",
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task UpdateFooLink_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.UpdateFooLinkAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            "T",
            "U",
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task UpdateFooLink_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
        => await FoosControllerTestsHelper.UpdateFooLinkAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            "T",
            "U",
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);
    #endregion

    #region DeleteFooLink
    [TestMethod]
    public async Task DeleteFooLink_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.DeleteFooLinkAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task DeleteFooLink_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.DeleteFooLinkAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task DeleteFooLink_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.DeleteFooLinkAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task DeleteFooLink_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.DeleteFooLinkAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task DeleteFooLink_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.DeleteFooLinkAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task DeleteFooLink_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
        => await FoosControllerTestsHelper.DeleteFooLinkAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            _linkGuidUnderTest,
            TestFactory.AValidRowVersion,
            HttpStatusCode.Forbidden);
    #endregion

    #region CreateFooComment
    [TestMethod]
    public async Task CreateFooComment_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.CreateFooCommentAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "T",
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task CreateFooComment_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.CreateFooCommentAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "T",
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task CreateFooComment_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.CreateFooCommentAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            "T",
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task CreateFooComment_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.CreateFooCommentAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            "T",
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task CreateFooComment_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.CreateFooCommentAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            "T",
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task CreateFooComment_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
        => await FoosControllerTestsHelper.CreateFooCommentAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            _fooGuidUnderTest,
            "T",
            HttpStatusCode.Forbidden);
    #endregion

    #region GetFooComments
    [TestMethod]
    public async Task GetFooComments_AsAnonymous_ShouldReturnUnauthorized()
        => await FoosControllerTestsHelper.GetFooCommentsAsync(
            UserType.Anonymous,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            HttpStatusCode.Unauthorized);

    [TestMethod]
    public async Task GetFooComments_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.GetFooCommentsAsync(
            UserType.NoPermissionUser,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task GetFooComments_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
        => await FoosControllerTestsHelper.GetFooCommentsAsync(
            UserType.Writer,
            TestFactory.Unknown,
            _fooGuidUnderTest,
            HttpStatusCode.BadRequest,
            "is not a valid plant");

    [TestMethod]
    public async Task GetFooComments_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.GetFooCommentsAsync(
            UserType.NoPermissionUser,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            HttpStatusCode.Forbidden);

    [TestMethod]
    public async Task GetFooComments_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
        => await FoosControllerTestsHelper.GetFooCommentsAsync(
            UserType.Writer,
            TestFactory.PlantWithoutAccess,
            _fooGuidUnderTest,
            HttpStatusCode.Forbidden);
    #endregion
}
