using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.EditFoo;

public class EditFooCommandValidator : AbstractValidator<EditFooCommand>
{
    public EditFooCommandValidator(IFooValidator fooValidator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            .Must(_ => MustBeAValidFoo())
            .WithMessage("Not a OK Foo!")
            .MustAsync((command, cancellationToken) => BeAnExistingFoo(command.FooId, cancellationToken))
            .WithMessage(command => $"Foo with this ID does not exist! Id={command.FooId}");

        bool MustBeAValidFoo()
            => fooValidator.FooIsOk();

        async Task<bool> BeAnExistingFoo(int fooId, CancellationToken cancellationToken)
            => await fooValidator.FooExistsAsync(fooId, cancellationToken);
    }
}
