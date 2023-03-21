using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.EditFoo;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.RowVersionValidators;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.EditFoo
{
    [TestClass]
    public class EditFooCommandValidatorTests
    {
        private readonly int _fooId = 1;
        private readonly string _rowVersion = "AAAAAAAAABA=";

        private EditFooCommandValidator _dut;
        private Mock<IFooValidator> _fooValidatorMock;
        private Mock<IRowVersionValidator> _rowVersionValidatorMock;

        private EditFooCommand _command;

        [TestInitialize]
        public void Setup_OkState()
        {
            _fooValidatorMock = new Mock<IFooValidator>();
            _fooValidatorMock.Setup(x => x.FooIsOk()).Returns(true);
            _fooValidatorMock.Setup(x => x.FooExistsAsync(_fooId, default))
                .ReturnsAsync(true);
            _command = new EditFooCommand(_fooId, "New title", _rowVersion);

            _rowVersionValidatorMock = new Mock<IRowVersionValidator>();
            _rowVersionValidatorMock.Setup(x => x.IsValid(_rowVersion)).Returns(true);

            _dut = new EditFooCommandValidator(
                _fooValidatorMock.Object, 
                _rowVersionValidatorMock.Object);
        }

        [TestMethod]
        public async Task Validate_ShouldBeValid_WhenOkState()
        {
            var result = await _dut.ValidateAsync(_command);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public async Task Validate_ShouldFail_TitleIsTooShort()
        {
            var result = await _dut.ValidateAsync(new EditFooCommand(_fooId, "t", _rowVersion));

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Title must be between {Foo.TitleMinLength} and {Foo.TitleMaxLength} characters!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_TitleIsTooLongAsync()
        {
            var result = await _dut.ValidateAsync(new EditFooCommand(
                _fooId,
                new string('x', Foo.TitleMaxLength + 1),
                _rowVersion));

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Title must be between {Foo.TitleMinLength} and {Foo.TitleMaxLength} characters!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_TitleIsNullAsync()
        {
            var result = await _dut.ValidateAsync(new EditFooCommand(_fooId, null, _rowVersion));

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Title must be between {Foo.TitleMinLength} and {Foo.TitleMaxLength} characters!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_When_FooNotOk()
        {
            _fooValidatorMock.Setup(inv => inv.FooIsOk()).Returns(false);

            var result = await _dut.ValidateAsync(_command);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Not a OK Foo!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_When_FooNotExists()
        {
            _fooValidatorMock.Setup(inv => inv.FooExistsAsync(_fooId, default))
                .ReturnsAsync(false);

            var result = await _dut.ValidateAsync(_command);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith("Foo with this ID does not exist!"));
        }
    }
}
