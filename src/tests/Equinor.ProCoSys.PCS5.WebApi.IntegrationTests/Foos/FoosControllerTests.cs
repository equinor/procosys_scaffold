using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos
{
    [TestClass]
    public class FoosControllerTests : TestBase
    {
        private int _fooIdUnderTest;

        [TestInitialize]
        public void TestInitialize()
            => _fooIdUnderTest = TestFactory.Instance.SeededData[KnownPlantData.PlantA].FooAId;

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
        public async Task UpdateFoo_AsWriter_ShouldUpdateFooAndRowVersion()
        {
            // Arrange
            var newTitle = Guid.NewGuid().ToString();
            var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooIdUnderTest);
            var currentRowVersion = foo.RowVersion;

            // Act
            var newRowVersion = await FoosControllerTestsHelper.UpdateFooAsync(
                UserType.Writer,
                TestFactory.PlantWithAccess,
                foo.Id,
                newTitle,
                currentRowVersion);

            // Assert
            AssertRowVersionChange(currentRowVersion, newRowVersion);
            foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooIdUnderTest);
            Assert.AreEqual(newTitle, foo.Title);
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
            var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, idAndRowVersion.Id);
            Assert.IsFalse(foo.IsVoided);

            // Act
            var currentRowVersion = await FoosControllerTestsHelper.VoidFooAsync(
                UserType.Writer,
                TestFactory.PlantWithAccess,
                idAndRowVersion.Id,
                idAndRowVersion.RowVersion);

            // Assert
            foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, idAndRowVersion.Id);
            Assert.IsTrue(foo.IsVoided);
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
            var currentRowVersion = await FoosControllerTestsHelper.VoidFooAsync(
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
                currentRowVersion);

            // Assert
            await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, idAndRowVersion.Id, HttpStatusCode.NotFound);
        }
    }
}
