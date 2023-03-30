using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Equinor.ProCoSys.PCS5.WebApi.Authorizations;
using Equinor.ProCoSys.PCS5.WebApi.Misc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Query.GetFooById;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations;

[TestClass]
public class AccessValidatorTests
{
    private AccessValidator _dut;
    private Mock<IProjectAccessChecker> _projectAccessCheckerMock;
    private Mock<ILogger<AccessValidator>> _loggerMock;
    private Mock<ICurrentUserProvider> _currentUserProviderMock;
    private readonly int _fooIdWithAccessToProject = 1;
    private readonly int _fooIdWithoutAccessToProject = 2;
    private readonly string _projectWithAccess = "TestProjectWithAccess";
    private readonly string _projectWithoutAccess = "TestProjectWithoutAccess";

    [TestInitialize]
    public void Setup()
    {
        _currentUserProviderMock = new Mock<ICurrentUserProvider>();

        _projectAccessCheckerMock = new Mock<IProjectAccessChecker>();

        _projectAccessCheckerMock.Setup(p => p.HasCurrentUserAccessToProject(_projectWithoutAccess)).Returns(false);
        _projectAccessCheckerMock.Setup(p => p.HasCurrentUserAccessToProject(_projectWithAccess)).Returns(true);

        var fooHelperMock = new Mock<IFooHelper>();
        fooHelperMock.Setup(p => p.GetProjectNameAsync(_fooIdWithAccessToProject))
            .ReturnsAsync(_projectWithAccess);
        fooHelperMock.Setup(p => p.GetProjectNameAsync(_fooIdWithoutAccessToProject))
            .ReturnsAsync(_projectWithoutAccess);

        _loggerMock = new Mock<ILogger<AccessValidator>>();

        _dut = new AccessValidator(
            _currentUserProviderMock.Object,
            _projectAccessCheckerMock.Object,
            fooHelperMock.Object,
            _loggerMock.Object);
    }

    #region Commands

    #region CreateFooCommand
    [TestMethod]
    public async Task ValidateAsync_OnCreateFooCommand_ShouldReturnTrue_WhenAccessToProject()
    {
        // Arrange
        var command = new CreateFooCommand("T", _projectWithAccess);
            
        // act
        var result = await _dut.ValidateAsync(command);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ValidateAsync_OnCreateFooCommand_ShouldReturnFalse_WhenNoAccessToProject()
    {
        // Arrange
        var command = new CreateFooCommand("T", _projectWithoutAccess);
            
        // act
        var result = await _dut.ValidateAsync(command);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion

    #endregion

    #region Queries

    #region GetFooByIdQuery
    [TestMethod]
    public async Task ValidateAsync_OnGetFooByIdQuery_ShouldReturnTrue_WhenAccessToProject()
    {
        // Arrange
        var query = new GetFooByIdQuery(_fooIdWithAccessToProject);

        // act
        var result = await _dut.ValidateAsync(query);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ValidateAsync_OnGetFooByIdQuery_ShouldReturnFalse_WhenNoAccessToProject()
    {
        // Arrange
        var query = new GetFooByIdQuery(_fooIdWithoutAccessToProject);

        // act
        var result = await _dut.ValidateAsync(query);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion

    #endregion
}