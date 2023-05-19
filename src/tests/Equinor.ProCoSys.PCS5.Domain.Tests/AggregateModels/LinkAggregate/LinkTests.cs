using System;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Domain.Tests.AggregateModels.LinkAggregate;

[TestClass]
public class LinkTests
{
    private Link _dut;
    private readonly string _testPlant = "PlantA";
    private readonly string _title = "Pro A";
    private readonly Guid _linkGuid = Guid.NewGuid();
    private readonly string _url = "Desc A";

    [TestInitialize]
    public void Setup() => _dut = new Link(_testPlant, _linkGuid, _title, _url);

    [TestMethod]
    public void Constructor_ShouldSetProperties()
    {
        Assert.AreEqual(_testPlant, _dut.Plant);
        Assert.AreEqual(_title, _dut.Title);
        Assert.AreEqual(_url, _dut.Url);
        Assert.AreEqual(_linkGuid, _dut.ProCoSysGuid);
    }
}
