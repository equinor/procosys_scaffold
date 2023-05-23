using System;
using System.Linq;
using Equinor.ProCoSys.Common.Time;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.PostSave;
using Equinor.ProCoSys.PCS5.Test.Common;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Domain.Tests.AggregateModels.FooAggregate;

[TestClass]
public class FooTests
{
    private Foo _dut;
    private readonly string _testPlant = "PlantA";
    private readonly int _projectId = 132;
    private Project _project;
    private readonly string _title = "Title A";

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

    [TestMethod]
    public void Constructor_ShouldAddFooCreatedPostEvent()
        => Assert.IsInstanceOfType(_dut.PostSaveDomainEvents.First(), typeof(FooCreatedEvent));
    #endregion

    #region Edit

    [TestMethod]
    public void EditFoo_ShouldEditFoo()
    {
        _dut.EditFoo("New Title", "New Text");

        Assert.AreEqual("New Title", _dut.Title);
        Assert.AreEqual("New Text", _dut.Text);
    }

    [TestMethod]
    public void EditFoo_ShouldAddFooEditedPostEvent()
    {
        _dut.EditFoo("New Title", "New Text");

        Assert.IsInstanceOfType(_dut.PostSaveDomainEvents.Last(), typeof(FooEditedEvent));
    }
    #endregion

    #region Void

    [TestMethod]
    public void Void_ShouldAddFooVoidedEvent_WhenNotAlreadyVoided()
    {
        // Arrange
        Assert.IsFalse(_dut.IsVoided);

        // Act
        _dut.IsVoided = true;

        // Assert
        Assert.IsTrue(_dut.IsVoided);
        Assert.AreEqual(2, _dut.PostSaveDomainEvents.Count);
        Assert.IsInstanceOfType(_dut.PostSaveDomainEvents.Last(), typeof(FooVoidedEvent));
    }

    [TestMethod]
    public void Void_ShouldNotAddAnotherFooVoidedEvent_WhenAlreadyVoided()
    {
        // Arrange
        _dut.IsVoided = true;
        Assert.IsTrue(_dut.IsVoided);
        Assert.AreEqual(2, _dut.PostSaveDomainEvents.Count);

        // Act
        _dut.IsVoided = true;

        // Assert
        Assert.AreEqual(2, _dut.PostSaveDomainEvents.Count);
    }

    #endregion

    #region Unvoid

    [TestMethod]
    public void Unvoid_ShouldAddFooUnvoidedEvent_WhenUnvoided()
    {
        // Arrange
        _dut.IsVoided = true;
        Assert.IsTrue(_dut.IsVoided);

        // Act
        _dut.IsVoided = false;

        // Assert
        Assert.IsFalse(_dut.IsVoided);
        Assert.AreEqual(3, _dut.PostSaveDomainEvents.Count);
        Assert.IsInstanceOfType(_dut.PostSaveDomainEvents.Last(), typeof(FooUnvoidedEvent));
    }

    [TestMethod]
    public void Unvoid_ShouldNotAddAnotherFooUnvoidedEvent_WhenAlreadyUnvoided()
    {
        // Arrange
        _dut.IsVoided = true;
        _dut.IsVoided = false;
        Assert.IsFalse(_dut.IsVoided);
        Assert.AreEqual(3, _dut.PostSaveDomainEvents.Count);

        // Act
        _dut.IsVoided = false;

        // Assert
        Assert.AreEqual(3, _dut.PostSaveDomainEvents.Count);
    }

    #endregion
}
