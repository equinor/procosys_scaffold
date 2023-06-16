using System;
using Equinor.ProCoSys.PCS5.WebApi.Authorizations;
using Equinor.ProCoSys.PCS5.WebApi.Misc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Equinor.ProCoSys.Common.Misc;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations;

[TestClass]
public class AccessValidatorTestBase
{
    protected AccessValidator _dut;
    protected readonly Guid FooGuidWithAccessToProject = new("679b7135-a1a8-4762-8b99-17f34f3a95a8");
    protected readonly Guid FooGuidWithoutAccessToProject = new("ea9efc61-8574-4a21-8a1a-14582ddff509");
    protected readonly string ProjectWithAccess = "TestProjectWithAccess";
    protected readonly string ProjectWithoutAccess = "TestProjectWithoutAccess";

    private Mock<IProjectAccessChecker> _projectAccessCheckerMock;
    private Mock<ILogger<AccessValidator>> _loggerMock;
    private Mock<ICurrentUserProvider> _currentUserProviderMock;

    [TestInitialize]
    public void Setup()
    {
        _currentUserProviderMock = new Mock<ICurrentUserProvider>();

        _projectAccessCheckerMock = new Mock<IProjectAccessChecker>();

        _projectAccessCheckerMock.Setup(p => p.HasCurrentUserAccessToProject(ProjectWithoutAccess)).Returns(false);
        _projectAccessCheckerMock.Setup(p => p.HasCurrentUserAccessToProject(ProjectWithAccess)).Returns(true);

        var fooHelperMock = new Mock<IFooHelper>();
        fooHelperMock.Setup(p => p.GetProjectNameAsync(FooGuidWithAccessToProject))
            .ReturnsAsync(ProjectWithAccess);
        fooHelperMock.Setup(p => p.GetProjectNameAsync(FooGuidWithoutAccessToProject))
            .ReturnsAsync(ProjectWithoutAccess);

        _loggerMock = new Mock<ILogger<AccessValidator>>();

        _dut = new AccessValidator(
            _currentUserProviderMock.Object,
            _projectAccessCheckerMock.Object,
            fooHelperMock.Object,
            _loggerMock.Object);
    }
}
