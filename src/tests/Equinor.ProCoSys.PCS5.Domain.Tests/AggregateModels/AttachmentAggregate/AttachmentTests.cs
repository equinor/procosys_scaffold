using System;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Domain.Tests.AggregateModels.AttachmentAggregate;

[TestClass]
public class AttachmentTests
{
    private Attachment _dut;
    private readonly string _sourceType = "X";
    private readonly Guid _sourceGuid = Guid.NewGuid();
    private readonly string _fileName = "a.txt";

    [TestInitialize]
    public void Setup() => _dut = new Attachment(_sourceType, _sourceGuid, "PCS$Plant", _fileName);

    [TestMethod]
    public void Constructor_ShouldSetProperties()
    {
        Assert.AreEqual(_fileName, _dut.FileName);
        Assert.AreEqual($"Plant/X/{_dut.Guid}", _dut.BlobPath);
        Assert.AreEqual(_sourceType, _dut.SourceType);
        Assert.AreEqual(_sourceGuid, _dut.SourceGuid);
        Assert.AreNotEqual(_sourceGuid, _dut.Guid);
        Assert.AreNotEqual(Guid.Empty, _dut.Guid);
    }

    [TestMethod]
    public void GetFullBlobPath_ShouldReturnFullBlobPath()
    {
        // Act
        var result = _dut.GetFullBlobPath();

        // Arrange
        Assert.AreEqual($"Plant/X/{_dut.Guid}/{_fileName}", result);
    }
}
