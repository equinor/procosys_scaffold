using Equinor.ProCoSys.PCS5.Domain.Audit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Domain.Tests.AggregateModels;

[TestClass]
public abstract class IModificationAuditableTests : ICreationAuditableTests
{
    protected abstract IModificationAuditable GetModificationAuditable();

    [TestMethod]
    public void SetModified_ShouldSetProperties()
    {
        // Arrange
        var dut = GetModificationAuditable();

        // Act
        dut.SetModified(_person);

        // Arrange
        Assert.AreEqual(_now, dut.ModifiedAtUtc);
        Assert.AreEqual(_person.Id, dut.ModifiedById);
        Assert.AreEqual(_person.Guid, dut.ModifiedByOid);
    }
}
