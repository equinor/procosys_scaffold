using System;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.Domain.Tests.AggregateModels.PersonAggregate
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void Constructor_SetsProperties()
        {
            var oid = new Guid("11111111-1111-2222-2222-333333333333");
            var dut = new Person(oid, "FirstName", "LastName", "UserName", "EmailAddress");
            Assert.AreEqual(oid, dut.Oid);
            Assert.AreEqual("FirstName", dut.FirstName);
            Assert.AreEqual("LastName", dut.LastName);
            Assert.AreEqual("UserName", dut.UserName);
            Assert.AreEqual("EmailAddress", dut.Email);
        }
    }
}
