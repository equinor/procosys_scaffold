using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure.Repositories;
using Equinor.ProCoSys.PCS5.Test.Common.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Tests.Repositories
{
    [TestClass]
    public class PersonRepositoryTests : RepositoryTestBase
    {
        private readonly int _personId = 5;
        private List<Person> _persons;
        private Mock<DbSet<Person>> _dbPersonSetMock;

        private PersonRepository _dut;
        private Person _person;

        [TestInitialize]
        public void Setup()
        {
            _person = new Person(
                new Guid("11111111-1111-2222-2222-333333333333"),
                "FirstName",
                "LastName",
                "UNAME",
                "email@test.com");
            _person.SetProtectedIdForTesting(_personId);

            _persons = new List<Person>
            {
                _person
            };

            _dbPersonSetMock = _persons.AsQueryable().BuildMockDbSet();

            ContextHelper
                .ContextMock
                .Setup(x => x.Persons)
                .Returns(_dbPersonSetMock.Object);

            _dut = new PersonRepository(ContextHelper.ContextMock.Object);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAllItems()
        {
            var result = await _dut.GetAllAsync();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetByIds_UnknownId_ShouldReturnEmptyList()
        {
            var result = await _dut.GetByIdsAsync(new List<int> { 1234 });

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task Exists_KnownId_ShouldReturnTrue()
        {
            var result = await _dut.Exists(_personId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Exists_UnknownId_ShouldReturnFalse()
        {
            var result = await _dut.Exists(1234);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetById_KnownId_ShouldReturnPerson()
        {
            var result = await _dut.GetByIdAsync(_personId);

            Assert.AreEqual(_personId, result.Id);
        }

        [TestMethod]
        public async Task GetById_UnknownId_ShouldReturnNull()
        {
            var result = await _dut.GetByIdAsync(1234);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Add_Person_ShouldCallAdd()
        {
            _dut.Add(_person);

            _dbPersonSetMock.Verify(x => x.Add(_person), Times.Once);
        }
    }
}
