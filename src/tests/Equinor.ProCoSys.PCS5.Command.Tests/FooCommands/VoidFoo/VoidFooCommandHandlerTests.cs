using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.VoidFoo;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.VoidFoo;

[TestClass]
public class VoidFooCommandHandlerTests : TestsBase
{
    private readonly string _rowVersion = "AAAAAAAAABA=";

    private Mock<IFooRepository> _fooRepositoryMock;
    private Foo _existingFoo;

    private VoidFooCommand _command;
    private VoidFooCommandHandler _dut;

    [TestInitialize]
    public void Setup()
    {
        var project = new Project(TestPlantA, Guid.NewGuid(), "P", "D");
        _existingFoo = new Foo(TestPlantA, project, "Foo");
        _fooRepositoryMock = new Mock<IFooRepository>();
        _fooRepositoryMock.Setup(r => r.GetByGuidAsync(_existingFoo.Guid))
            .ReturnsAsync(_existingFoo);

        _command = new VoidFooCommand(_existingFoo.Guid, _rowVersion);

        _dut = new VoidFooCommandHandler(
            _fooRepositoryMock.Object,
            _unitOfWorkMock.Object,
            new Mock<ILogger<VoidFooCommandHandler>>().Object);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldVoidFoo()
    {
        // Arrange
        Assert.IsFalse(_existingFoo.IsVoided);

        // Act
        await _dut.Handle(_command, default);

        // Assert
        Assert.IsTrue(_existingFoo.IsVoided);
    }

    [TestMethod]
    public async Task HandlingCommand_ShouldSave()
    {
        // Act
        await _dut.Handle(_command, default);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }
}
