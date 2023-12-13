using System;
using Equinor.ProCoSys.Common.Time;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Audit;
using Equinor.ProCoSys.PCS5.Test.Common;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Domain.Tests.AggregateModels.FooAggregate;

[TestClass]
public class FooTests : IModificationAuditableTests
{
    private Foo _dut;
    private readonly string _testPlant = "PlantA";
    private readonly int _projectId = 132;
    private Project _project;
    private readonly string _title = "Title A";
    
    protected override ICreationAuditable GetCreationAuditable() => _dut;
    protected override IModificationAuditable GetModificationAuditable() => _dut;

    [TestInitialize]
    public void Setup()
    {
        _project = new(_testPlant, Guid.NewGuid(), "P", "D");
        _project.SetProtectedIdForTesting(_projectId);
        TimeService.SetProvider(
            new ManualTimeProvider(new DateTime(2021, 1, 1, 12, 0, 0, DateTimeKind.Utc)));

        _dut = new Foo(_testPlant, _project, _title); 
    }

    #region Constructor
    [TestMethod]
    public void Constructor_ShouldSetProperties()
    {
        Assert.AreEqual(_testPlant, _dut.Plant);
        Assert.AreEqual(_projectId, _dut.ProjectId);
        Assert.AreEqual(_title, _dut.Title);
    }

    [TestMethod]
    public void Constructor_ShouldThrowException_WhenProjectNotGiven() =>
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Foo(_testPlant, null!, _title));

    [TestMethod]
    public void Constructor_ShouldThrowException_WhenProjectInOtherPlant()
        => Assert.ThrowsException<ArgumentException>(() =>
            new Foo(_testPlant, new Project("OtherPlant", Guid.NewGuid(), "P", "D"), _title));
    #endregion

    #region Update

    [TestMethod]
    public void Update_ShouldUpdate()
    {
        _dut.Update("New Title", "New Text");

        Assert.AreEqual("New Title", _dut.Title);
        Assert.AreEqual("New Text", _dut.Text);
    }
    #endregion
}
