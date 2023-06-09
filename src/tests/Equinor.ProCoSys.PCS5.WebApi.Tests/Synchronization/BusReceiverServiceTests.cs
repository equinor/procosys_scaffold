using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.Common.Telemetry;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Equinor.ProCoSys.PCS5.WebApi.Synchronization;
using Equinor.ProCoSys.Auth.Authentication;
using Equinor.ProCoSys.PCS5.WebApi.Authentication;
using Microsoft.Extensions.Options;
using Equinor.ProCoSys.PcsServiceBus.Topics;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Synchronization;

[TestClass]
public class BusReceiverServiceTests
{
    private BusReceiverService _dut;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IPlantSetter> _plantSetter;
    private Mock<ITelemetryClient> _telemetryClient;
    private Mock<ICurrentUserSetter> _currentUserSetter;
    private Mock<IProjectRepository> _projectRepository;
    private readonly string _plant = "Plant";
    private readonly Guid _projectProCoSysGuid = Guid.NewGuid();
    private Project _project1;

    [TestInitialize]
    public void Setup()
    {
        _plantSetter = new Mock<IPlantSetter>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _telemetryClient = new Mock<ITelemetryClient>();
        _currentUserSetter = new Mock<ICurrentUserSetter>();
        _project1 = new Project(_plant, _projectProCoSysGuid, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        _projectRepository = new Mock<IProjectRepository>();
        _projectRepository.Setup(p => p.TryGetByGuidAsync(_projectProCoSysGuid))
            .ReturnsAsync(_project1);

        var options = new Mock<IOptionsSnapshot<PCS5AuthenticatorOptions>>();
        options.Setup(s => s.Value).Returns(new PCS5AuthenticatorOptions { PCS5ApiObjectId = Guid.NewGuid() });

        _dut = new BusReceiverService(_plantSetter.Object,
            _unitOfWork.Object,
            _telemetryClient.Object,
            new Mock<IMainApiAuthenticator>().Object,
            options.Object,
            _currentUserSetter.Object,
            _projectRepository.Object);

    }

    #region Project
    [TestMethod]
    public async Task HandlingProjectTopic_ShouldUpdateProject_WhenKnownGuid()
    {
        // Arrange
        var message = new ProjectTopic
        {
            Behavior = "",
            ProjectName = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            IsClosed = true,
            Plant = _plant,
            ProCoSysGuid = _projectProCoSysGuid
        };
        var messageJson = JsonSerializer.Serialize(message);
        Assert.IsFalse(_project1.IsClosed);

        // Act
        await _dut.ProcessMessageAsync("Project", messageJson, default);

        // Assert
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _plantSetter.Verify(p => p.SetPlant(_plant), Times.Once);
        _projectRepository.Verify(i => i.TryGetByGuidAsync(_projectProCoSysGuid), Times.Once);
        Assert.AreEqual(message.ProjectName, _project1.Name);
        Assert.AreEqual(message.Description, _project1.Description);
        Assert.IsTrue(_project1.IsClosed);
    }

    [TestMethod]
    public async Task HandlingProjectTopic_ShouldNotAffectProject_WhenUnknownGuid()
    {
        // Arrange
        var message = new ProjectTopic
        {
            Behavior = "",
            ProjectName = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            IsClosed = true,
            Plant = _plant,
            ProCoSysGuid = _projectProCoSysGuid
        };
        var messageJson = JsonSerializer.Serialize(message);
        _projectRepository.Setup(p => p.TryGetByGuidAsync(_projectProCoSysGuid))
            .ReturnsAsync((Project)null);
        Assert.IsFalse(_project1.IsClosed);
        var oldName = _project1.Name;
        var oldDescription = _project1.Description;

        // Act
        await _dut.ProcessMessageAsync("Project", messageJson, default);

        // Assert
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _plantSetter.Verify(p => p.SetPlant(_plant), Times.Once);
        _projectRepository.Verify(i => i.TryGetByGuidAsync(_projectProCoSysGuid), Times.Once);
        Assert.AreEqual(oldName, _project1.Name);
        Assert.AreEqual(oldDescription, _project1.Description);
        Assert.IsFalse(_project1.IsClosed);
    }

    [TestMethod]
    public async Task HandlingProjectTopic_ShouldSkipDeleteBehavior()
    {
        // Arrange
        var message = new ProjectTopic
        {
            Behavior = "delete"
        };
        var messageJson = JsonSerializer.Serialize(message);

        // Act
        await _dut.ProcessMessageAsync("Project", messageJson, default);

        // Assert
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _plantSetter.Verify(p => p.SetPlant(It.IsAny<string>()), Times.Never);
        _projectRepository.Verify(i => i.TryGetByGuidAsync(It.IsAny<Guid>()), Times.Never);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task HandlingProjectTopic_ShouldFail_WhenMissingPlant()
    {
        // Arrange
        var message = new ProjectTopic
        {
            Behavior = "",
            ProjectName = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            IsClosed = true,
            ProCoSysGuid = _projectProCoSysGuid
        };
        var messageJson = JsonSerializer.Serialize(message);

        // Act
        await _dut.ProcessMessageAsync("Project", messageJson, default);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task HandlingProjectTopic_ShouldFailIfEmptyMessage()
    {
        // Arrange
        var messageJson = "{}";

        // Act
        await _dut.ProcessMessageAsync("Project", messageJson, default);
    }

    [TestMethod]
    [ExpectedException(typeof(JsonException))]
    public async Task HandlingProjectTopic_ShouldFailIfBlankMessage()
    {
        // Arrange
        var messageJson = "";

        // Act
        await _dut.ProcessMessageAsync("Project", messageJson, default);
    }
    #endregion
}
