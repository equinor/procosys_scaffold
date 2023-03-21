using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.RowVersionValidators;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.EditFoo
{
    public class EditFooCommandValidator : AbstractValidator<EditFooCommand>
    {
        public EditFooCommandValidator(IFooValidator fooValidator, IRowVersionValidator rowVersionValidator)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => command)
                //input validators
                .Must(command =>
                    command.Title != null &&
                    command.Title.Length >= Foo.TitleMinLength &&
                    command.Title.Length < Foo.TitleMaxLength)
                .WithMessage(command =>
                    $"Title must be between {Foo.TitleMinLength} and {Foo.TitleMaxLength} characters! Title={command.Title}")
                //business validators
                .Must(_ => MustBeAValidFoo())
                    .WithMessage("Not a OK Foo!")
                .MustAsync((command, cancellationToken) => BeAnExistingFoo(command.FooId, cancellationToken))
                    .WithMessage(command => $"Foo with this ID does not exist! Id={command.FooId}")
                .Must(command => HaveAValidRowVersion(command.RowVersion))
                    .WithMessage(command => $"Foo does not have valid rowVersion! RowVersion={command.RowVersion}");

            bool MustBeAValidFoo()
                => fooValidator.FooIsOk();

            async Task<bool> BeAnExistingFoo(int fooId, CancellationToken cancellationToken)
                => await fooValidator.FooExistsAsync(fooId, cancellationToken);

            bool HaveAValidRowVersion(string? rowVersion)
                => rowVersionValidator.IsValid(rowVersion);
        }
    }
}
