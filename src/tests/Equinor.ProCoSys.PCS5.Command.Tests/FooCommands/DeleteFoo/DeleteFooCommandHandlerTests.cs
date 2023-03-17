using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.DeleteFoo
{
    [TestClass]
    public class DeleteFooCommandHandlerTests : CommandHandlerTestsBase
    {
        private readonly int _fooId = 1;
        private readonly string _rowVersion = "AAAAAAAAABA=";

        private Mock<IFooRepository> _fooRepositoryMock;
        private Foo _existingFoo;

        private DeleteFooCommand _command;
        private DeleteFooCommandHandler _dut;

        [TestInitialize]
        public void Setup()
        {
            var project = new Project(TestPlant, Guid.NewGuid(), "P", "D");
            _existingFoo = new Foo(TestPlant, project, "Foo");
            _existingFoo.SetProtectedIdForTesting(_fooId);
            _fooRepositoryMock = new Mock<IFooRepository>();
            _fooRepositoryMock.Setup(r => r.GetByIdAsync(_existingFoo.Id))
                .ReturnsAsync(_existingFoo);

            _command = new DeleteFooCommand(_fooId, _rowVersion);

            _dut = new DeleteFooCommandHandler(
                _fooRepositoryMock.Object,
                UnitOfWorkMock.Object,
                new Mock<ILogger<DeleteFooCommandHandler>>().Object);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldDeleteFooFromRepository()
        {
            // Act
            await _dut.Handle(_command, default);

            // Assert
            _fooRepositoryMock.Verify(r => r.Remove(_existingFoo), Times.Once);
        }

        [TestMethod]
        public async Task HandlingCommand_ShouldSave()
        {
            // Act
            await _dut.Handle(_command, default);

            // Assert
            UnitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
