using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.RowVersionValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo
{
    public class DeleteFooCommandValidator : AbstractValidator<DeleteFooCommand>
    {
        public DeleteFooCommandValidator(IFooValidator fooValidator, IRowVersionValidator rowVersionValidator)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => command)
                //business validators
                .MustAsync((command, cancellationToken) => BeAnExistingFoo(command.FooId, cancellationToken))
                    .WithMessage(command => $"Foo with this ID does not exist! Id={command.FooId}")
                .MustAsync((command, cancellationToken) => BeAVoidedFoo(command.FooId, cancellationToken))
                    .WithMessage("Foo must be voided before delete!")
                .Must(command => HaveAValidRowVersion(command.RowVersion))
                    .WithMessage(command => $"Foo does not have valid rowVersion! RowVersion={command.RowVersion}");

            async Task<bool> BeAnExistingFoo(int fooId, CancellationToken cancellationToken)
                => await fooValidator.FooExistsAsync(fooId, cancellationToken);

            async Task<bool> BeAVoidedFoo(int fooId, CancellationToken cancellationToken)
                => await fooValidator.FooIsVoidedAsync(fooId, cancellationToken);

            bool HaveAValidRowVersion(string rowVersion)
                => rowVersionValidator.IsValid(rowVersion);
        }
    }
}
