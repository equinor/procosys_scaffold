using System;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Equinor.ProCoSys.PCS5.WebApi.Authorizations;
using Equinor.ProCoSys.PCS5.WebApi.Misc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooLink;
using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoo;
using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooLinks;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations;

[TestClass]
public class AccessValidatorTests
{
    private AccessValidator _dut;
    private Mock<IProjectAccessChecker> _projectAccessCheckerMock;
    private Mock<ILogger<AccessValidator>> _loggerMock;
    private Mock<ICurrentUserProvider> _currentUserProviderMock;
    private readonly Guid _fooGuidWithAccessToProject = new("679b7135-a1a8-4762-8b99-17f34f3a95a8");
    private readonly Guid _fooGuidWithoutAccessToProject = new("ea9efc61-8574-4a21-8a1a-14582ddff509");
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
        fooHelperMock.Setup(p => p.GetProjectNameAsync(_fooGuidWithAccessToProject))
            .ReturnsAsync(_projectWithAccess);
        fooHelperMock.Setup(p => p.GetProjectNameAsync(_fooGuidWithoutAccessToProject))
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

    #region CreateFooLinkCommand
    [TestMethod]
    public async Task ValidateAsync_OnCreateFooLinkCommand_ShouldReturnTrue_WhenAccessToProjectForFoo()
    {
        // Arrange
        var command = new CreateFooLinkCommand(_fooGuidWithAccessToProject, "T", "U");

        // act
        var result = await _dut.ValidateAsync(command);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ValidateAsync_OnCreateFooLinkCommand_ShouldReturnFalse_WhenNoAccessToProjectForFoo()
    {
        // Arrange
        var command = new CreateFooLinkCommand(_fooGuidWithoutAccessToProject, "T", "U");

        // act
        var result = await _dut.ValidateAsync(command);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion

    #endregion

    #region Queries

    #region GetFooQuery
    [TestMethod]
    public async Task ValidateAsync_OnGetFooQuery_ShouldReturnTrue_WhenAccessToProject()
    {
        // Arrange
        var query = new GetFooQuery(_fooGuidWithAccessToProject);

        // act
        var result = await _dut.ValidateAsync(query);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ValidateAsync_OnGetFooQuery_ShouldReturnFalse_WhenNoAccessToProject()
    {
        // Arrange
        var query = new GetFooQuery(_fooGuidWithoutAccessToProject);

        // act
        var result = await _dut.ValidateAsync(query);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion


    #region GetFooQuery
    [TestMethod]
    public async Task ValidateAsync_OnGetFooLinksQuery_ShouldReturnTrue_WhenAccessToProject()
    {
        // Arrange
        var query = new GetFooLinksQuery(_fooGuidWithAccessToProject);

        // act
        var result = await _dut.ValidateAsync(query);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ValidateAsync_OnGetFooLinksQuery_ShouldReturnFalse_WhenNoAccessToProject()
    {
        // Arrange
        var query = new GetFooLinksQuery(_fooGuidWithoutAccessToProject);

        // act
        var result = await _dut.ValidateAsync(query);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion

    #endregion
}
