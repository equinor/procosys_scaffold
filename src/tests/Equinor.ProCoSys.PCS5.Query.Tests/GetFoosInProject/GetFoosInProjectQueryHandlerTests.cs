using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceResult;
using Equinor.ProCoSys.PCS5.Query.GetFoosInProject;

namespace Equinor.ProCoSys.PCS5.Query.Tests.GetFoosInProject
{
    [TestClass]
    public class GetFoosInProjectQueryHandlerTests : ReadOnlyTestsBase
    {
        private readonly string _titleA = "Foo A";
        
        private Foo _fooInProjectA;
        private Foo _fooInProjectB;
        private int _fooAId;

        protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
        {
            using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

            _fooInProjectA = new Foo(TestPlantA, _projectA, _titleA);
            _fooInProjectB = new Foo(TestPlantA, _projectB, "Title");

            context.Foos.Add(_fooInProjectA);
            context.Foos.Add(_fooInProjectB);
            context.SaveChangesAsync().Wait();
            _fooAId = _fooInProjectA.Id;
        }

        [TestMethod]
        public async Task Handler_ShouldReturnEmptyList_IfNoneFound()
        {
            await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

            var query = new GetFoosInProjectQuery("UnknownProject");
            var dut = new GetFoosInProjectQueryHandler(context);

            var result = await dut.Handle(query, default);

            Assert.IsNotNull(result);
            Assert.AreEqual(ResultType.Ok, result.ResultType);
            Assert.AreEqual(0, result.Data.Count());
        }

        [TestMethod]
        public async Task Handler_ShouldReturnCorrectFoos()
        {
            await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
            
            var query = new GetFoosInProjectQuery(_projectA.Name);
            var dut = new GetFoosInProjectQueryHandler(context);

            var result = await dut.Handle(query, default);

            Assert.IsNotNull(result);
            Assert.AreEqual(ResultType.Ok, result.ResultType);
            Assert.AreEqual(1, result.Data.Count());

            AssertFoo(result.Data.Single(), _fooInProjectA);
        }

        private void AssertFoo(FooDto fooDto, Foo foo)
        {
            Assert.AreEqual(foo.Title, fooDto.Title);
            Assert.IsFalse(foo.IsVoided);
            var project = GetProjectById(foo.ProjectId);
            Assert.AreEqual(project.Name, fooDto.ProjectName);
        }
    }
}
