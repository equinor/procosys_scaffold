﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos
{
    [TestClass]
    public class FoosControllerNegativeTests : TestBase
    {
        private int _fooIdUnderTest;

        [TestInitialize]
        public void TestInitialize()
            => _fooIdUnderTest
                = TestFactory.Instance.SeededData[KnownPlantData.PlantA].FooAId;
        
        #region Get
        [TestMethod]
        public async Task GetFoo_AsAnonymous_ShouldReturnUnauthorized()
            => await FoosControllerTestsHelper.GetFooAsync(
                UserType.Anonymous,
                TestFactory.Unknown,
                _fooIdUnderTest,
                HttpStatusCode.Unauthorized);

        [TestMethod]
        public async Task GetFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
            => await FoosControllerTestsHelper.GetFooAsync(
                UserType.NoPermissionUser,
                TestFactory.Unknown,
                _fooIdUnderTest,
                HttpStatusCode.BadRequest,
                "is not a valid plant");

        [TestMethod]
        public async Task GetFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
            => await FoosControllerTestsHelper.GetFooAsync(
                UserType.Writer,
                TestFactory.Unknown,
                _fooIdUnderTest, 
                HttpStatusCode.BadRequest,
                "is not a valid plant");

        [TestMethod]
        public async Task GetFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
            => await FoosControllerTestsHelper.GetFooAsync(
                UserType.NoPermissionUser,
                TestFactory.PlantWithoutAccess,
                _fooIdUnderTest, 
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task GetFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
            => await FoosControllerTestsHelper.GetFooAsync(
                UserType.Writer,
                TestFactory.PlantWithoutAccess, 
                _fooIdUnderTest, 
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task GetFoo_AsWriter_ShouldReturnNotFound_WhenUnknownFoo()
            => await FoosControllerTestsHelper.GetFooAsync(
                UserType.Writer,
                TestFactory.PlantWithAccess, 
                9999, 
                HttpStatusCode.NotFound);
        #endregion

        #region GetInProject
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

        #region Create
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
        
        #region Void
        [TestMethod]
        public async Task VoidFoo_AsAnonymous_ShouldReturnUnauthorized()
            => await FoosControllerTestsHelper.VoidFooAsync(
                UserType.Anonymous, 
                TestFactory.Unknown,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.Unauthorized);

        [TestMethod]
        public async Task VoidFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
            => await FoosControllerTestsHelper.VoidFooAsync(
                UserType.NoPermissionUser,
                TestFactory.Unknown,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.BadRequest,
                "is not a valid plant");

        [TestMethod]
        public async Task VoidFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
            => await FoosControllerTestsHelper.VoidFooAsync(
                UserType.Writer,
                TestFactory.Unknown,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.BadRequest,
                "is not a valid plant");

        [TestMethod]
        public async Task VoidFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
            => await FoosControllerTestsHelper.VoidFooAsync(
                UserType.NoPermissionUser,
                TestFactory.PlantWithoutAccess,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task VoidFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
            => await FoosControllerTestsHelper.VoidFooAsync(
                UserType.Writer,
                TestFactory.PlantWithoutAccess,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task VoidFoo_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
            => await FoosControllerTestsHelper.VoidFooAsync(
                UserType.Reader,
                TestFactory.PlantWithAccess,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task VoidFoo_AsWriter_ShouldReturnConflict_WhenWrongRowVersion()
            => await FoosControllerTestsHelper.VoidFooAsync(
                UserType.Writer,
                TestFactory.PlantWithAccess,
                _fooIdUnderTest,
                TestFactory.WrongButValidRowVersion,
                HttpStatusCode.Conflict);

        #endregion

        #region Update
        [TestMethod]
        public async Task UpdateFoo_AsAnonymous_ShouldReturnUnauthorized()
            => await FoosControllerTestsHelper.UpdateFooAsync(
                UserType.Anonymous,
                TestFactory.Unknown,
                _fooIdUnderTest,
                "Foo1",
                TestFactory.AValidRowVersion,
                HttpStatusCode.Unauthorized);

        [TestMethod]
        public async Task UpdateFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
            => await FoosControllerTestsHelper.UpdateFooAsync(
                UserType.NoPermissionUser,
                TestFactory.Unknown,
                _fooIdUnderTest,
                "Foo1",
                TestFactory.AValidRowVersion,
                HttpStatusCode.BadRequest,
                "is not a valid plant");

        [TestMethod]
        public async Task UpdateFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
            => await FoosControllerTestsHelper.UpdateFooAsync(
                UserType.Writer,
                TestFactory.Unknown,
                _fooIdUnderTest,
                "Foo1",
                TestFactory.AValidRowVersion,
                HttpStatusCode.BadRequest,
                "is not a valid plant");

        [TestMethod]
        public async Task UpdateFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
            => await FoosControllerTestsHelper.UpdateFooAsync(
                UserType.NoPermissionUser,
                TestFactory.PlantWithoutAccess,
                _fooIdUnderTest,
                "Foo1",
                TestFactory.AValidRowVersion,
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task UpdateFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
            => await FoosControllerTestsHelper.UpdateFooAsync(
                UserType.Writer,
                TestFactory.PlantWithoutAccess,
                _fooIdUnderTest,
                "Foo1",
                TestFactory.AValidRowVersion,
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task UpdateFoo_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
            => await FoosControllerTestsHelper.UpdateFooAsync(
                UserType.Reader,
                TestFactory.PlantWithAccess,
                _fooIdUnderTest,
                "Foo1",
                TestFactory.AValidRowVersion,
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task UpdateFoo_AsWriter_ShouldReturnConflict_WhenWrongRowVersion()
            => await FoosControllerTestsHelper.UpdateFooAsync(
                UserType.Writer,
                TestFactory.PlantWithAccess,
                _fooIdUnderTest,
                Guid.NewGuid().ToString(),
                TestFactory.WrongButValidRowVersion,
                HttpStatusCode.Conflict);

        #endregion

        #region Delete
        [TestMethod]
        public async Task DeleteFoo_AsAnonymous_ShouldReturnUnauthorized()
            => await FoosControllerTestsHelper.DeleteFooAsync(
                UserType.Anonymous,
                TestFactory.Unknown,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.Unauthorized);

        [TestMethod]
        public async Task DeleteFoo_AsNoPermissionUser_ShouldReturnBadRequest_WhenUnknownPlant()
            => await FoosControllerTestsHelper.DeleteFooAsync(
                UserType.NoPermissionUser, TestFactory.Unknown,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.BadRequest,
                "is not a valid plant");

        [TestMethod]
        public async Task DeleteFoo_AsWriter_ShouldReturnBadRequest_WhenUnknownPlant()
            => await FoosControllerTestsHelper.DeleteFooAsync(
                UserType.Writer,
                TestFactory.Unknown,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.BadRequest,
                "is not a valid plant");

        [TestMethod]
        public async Task DeleteFoo_AsNoPermissionUser_ShouldReturnForbidden_WhenNoAccessToPlant()
            => await FoosControllerTestsHelper.DeleteFooAsync(
                UserType.NoPermissionUser,
                TestFactory.PlantWithoutAccess,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task DeleteFoo_AsWriter_ShouldReturnForbidden_WhenNoAccessToPlant()
            => await FoosControllerTestsHelper.DeleteFooAsync(
                UserType.Writer,
                TestFactory.PlantWithoutAccess,
                _fooIdUnderTest,
                TestFactory.AValidRowVersion,
                HttpStatusCode.Forbidden);

        [TestMethod]
        public async Task DeleteFoo_AsReader_ShouldReturnForbidden_WhenPermissionMissing()
            => await FoosControllerTestsHelper.DeleteFooAsync(
                UserType.Reader,
                TestFactory.PlantWithAccess,
                _fooIdUnderTest,
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
                idAndRowVersion.Id,
                idAndRowVersion.RowVersion);
            // Act

            await FoosControllerTestsHelper.DeleteFooAsync(
               UserType.Writer,
               TestFactory.PlantWithAccess,
               idAndRowVersion.Id,
               TestFactory.WrongButValidRowVersion,
               HttpStatusCode.Conflict);
        }
        #endregion
    }
}
