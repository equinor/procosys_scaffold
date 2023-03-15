using System;
using System.Linq;
using Equinor.ProCoSys.Common.Time;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Test.Common;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Domain.Tests.AggregateModels.FooAggregate
{
    [TestClass]
    public class FooTests
    {
        private Foo _dut;
        private readonly string _testPlant = "PlantA";
        private readonly string _projectName = "ProjectName";
        private readonly int _projectId = 132;
        private Project _project;
        private readonly string _title = "Title A";

        [TestInitialize]
        public void Setup()
        {
            _project = new(_testPlant, _projectName, $"Description of {_projectName} project");
            _project.SetProtectedIdForTesting(_projectId);
            TimeService.SetProvider(new ManualTimeProvider(new DateTime(2021, 1, 1, 12, 0, 0, DateTimeKind.Utc)));

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
        public void Constructor_ShouldThrowException_WhenTitleNotGiven() =>
            Assert.ThrowsException<ArgumentNullException>(() =>
                new Foo(_testPlant, _project, null));

        [TestMethod]
        public void Constructor_ShouldThrowException_WhenProjectNotGiven() =>
            Assert.ThrowsException<ArgumentNullException>(() =>
                new Foo(_testPlant, null, _title));

        [TestMethod]
        public void Constructor_ShouldThrowException_WhenProjectInOtherPlant()
            => Assert.ThrowsException<ArgumentException>(() =>
                 new Foo(_testPlant, new Project("OtherPlant", "P", "D"), _title));

        [TestMethod]
        public void Constructor_ShouldAddFooCreatedPreEvent()
            => Assert.IsInstanceOfType(_dut.PreSaveDomainEvents.First(), typeof(Events.PreSave.FooCreatedEvent));

        [TestMethod]
        public void Constructor_ShouldAddFooCreatedPostEvent()
            => Assert.IsInstanceOfType(_dut.PostSaveDomainEvents.First(), typeof(Events.PostSave.FooCreatedEvent));
        #endregion

        #region Edit

        [TestMethod]
        public void EditFoo_ShouldEditFoo()
        {
            _dut.EditFoo("New Title");

            Assert.AreEqual("New Title", _dut.Title);
        }

        [TestMethod]
        public void EditFoo_ShouldAddFooEditedEvent()
        {
            _dut.EditFoo("New Title");

            Assert.IsInstanceOfType(_dut.PreSaveDomainEvents.Last(), typeof(Events.PreSave.FooEditedEvent));
        }
        #endregion
    }
}
