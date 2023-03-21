using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Test.Common;
using Equinor.ProCoSys.PCS5.WebApi.Misc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Misc
{
    [TestClass]
    public class FooHelperTests : ReadOnlyTestsBase
    {
        private int _fooId;

        protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
        {
            using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
                
            // Save to get real id on project
            context.SaveChangesAsync().Wait();

            var foo = new Foo(TestPlantA, _projectA, "Title");
            context.Foos.Add(foo);
            context.SaveChangesAsync().Wait();
            _fooId = foo.Id;
        }

        [TestMethod]
        public async Task GetProjectName_KnownFooId_ShouldReturnProjectName()
        {
            // Arrange
            await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
            var dut = new FooHelper(context);

            // Act
            var projectName = await dut.GetProjectNameAsync(_fooId);

            // Assert
            Assert.AreEqual(_projectA.Name, projectName);
        }

        [TestMethod]
        public async Task GetProjectName_UnKnownFooId_ShouldReturnNull()
        {
            // Arrange
            await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
            var dut = new FooHelper(context);

            // Act
            var projectName = await dut.GetProjectNameAsync(0);

            // Assert
            Assert.IsNull(projectName);
        }
    }
}
