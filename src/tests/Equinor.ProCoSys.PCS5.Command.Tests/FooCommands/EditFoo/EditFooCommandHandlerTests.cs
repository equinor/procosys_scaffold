using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Command.FooCommands.EditFoo;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.EditFoo
{
    [TestClass]
    public class EditFooCommandHandlerTests : CommandHandlerTestsBase
    {
        private readonly int _fooId = 1;
        private readonly string _newTitle = "FooUpdated";
        private readonly string _existingTitle = "OldFoo";
        private readonly string _rowVersion = "AAAAAAAAABA=";

        private Mock<IFooRepository> _fooRepositoryMock;
        private Foo _existingFoo;

        private EditFooCommand _command;
        private EditFooCommandHandler _dut;

        [TestInitialize]
        public void Setup()
        {
            var project = new Project(TestPlant, "P", "D");
            _existingFoo = new Foo(TestPlant, project, _existingTitle);
            _existingFoo.SetProtectedIdForTesting(_fooId);
            _fooRepositoryMock = new Mock<IFooRepository>();
            _fooRepositoryMock.Setup(r => r.GetByIdAsync(_existingFoo.Id))
                .Returns(Task.FromResult(_existingFoo));

            _command = new EditFooCommand(_fooId, _newTitle, _rowVersion);

            _dut = new EditFooCommandHandler(
                _fooRepositoryMock.Object,
                UnitOfWorkMock.Object,
                new Mock<ILogger<EditFooCommandHandler>>().Object);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldUpdateFoo()
        {
            Assert.AreEqual(_existingTitle, _existingFoo.Title);

            await _dut.Handle(_command, default);

            Assert.AreEqual(_newTitle, _existingFoo.Title);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldSave()
        {
            await _dut.Handle(_command, default);

            UnitOfWorkMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldSetAndReturnRowVersion()
        {
            // Act
            var result = await _dut.Handle(_command, default);

            // Assert
            // In real life EF Core will create a new RowVersion when save.
            // Since UnitOfWorkMock is a Mock this will not happen here, so we assert that RowVersion is set from command
            Assert.AreEqual(_rowVersion, result.Data);
            Assert.AreEqual(_rowVersion, _existingFoo.RowVersion.ConvertToString());
        }
    }
}
