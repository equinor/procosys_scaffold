using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.FooCommands.CreateFoo
{
    [TestClass]
    public class CreateFooCommandValidatorTests
    {
        private CreateFooCommandValidator _dut;
        private CreateFooCommand _command;
        private Mock<IFooValidator> _fooValidatorMock;
        private readonly string _projectName = "Project name";
        private readonly string _title = "Test title";

        [TestInitialize]
        public void Setup_OkState()
        {
            _fooValidatorMock = new Mock<IFooValidator>();
            _fooValidatorMock.Setup(inv => inv.FooIsOk()).Returns(true);
            _command = new CreateFooCommand(_title, _projectName);
            _dut = new CreateFooCommandValidator(_fooValidatorMock.Object);
        }

        [TestMethod]
        public async Task Validate_ShouldBeValid_WhenOkState()
        {
            var result = await _dut.ValidateAsync(_command);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public async Task Validate_ShouldFail_WhenProjectNameIsTooShort()
        {
            var result = await _dut.ValidateAsync(new CreateFooCommand(_title, "p"));

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Project name must be between {Foo.ProjectNameMinLength} and {Foo.ProjectNameMaxLength} characters!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_WhenProjectNameIsTooLongAsync()
        {
            var result = await _dut.ValidateAsync(new CreateFooCommand(
                _title, 
                new string('x', Foo.ProjectNameMaxLength + 1)));

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Project name must be between {Foo.ProjectNameMinLength} and {Foo.ProjectNameMaxLength} characters!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_WhenProjectNameIsNullAsync()
        {
            var result = await _dut.ValidateAsync(new CreateFooCommand(_title, null));

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Project name must be between {Foo.ProjectNameMinLength} and {Foo.ProjectNameMaxLength} characters!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_TitleIsTooShort()
        {
            var result = await _dut.ValidateAsync(new CreateFooCommand("t", _projectName));

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Title must be between {Foo.TitleMinLength} and {Foo.TitleMaxLength} characters!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_TitleIsTooLongAsync()
        {
            var result = await _dut.ValidateAsync(new CreateFooCommand(
                new string('x', Foo.TitleMaxLength + 1),
                _projectName));

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors[0].ErrorMessage.StartsWith($"Title must be between {Foo.TitleMinLength} and {Foo.TitleMaxLength} characters!"));
        }

        [TestMethod]
        public async Task Validate_ShouldFail_TitleIsNullAsync()
        {
            var result = await _dut.ValidateAsync(new CreateFooCommand(null, _projectName));

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
    }
}
