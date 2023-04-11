using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;

public class CreateFooCommandValidator : AbstractValidator<CreateFooCommand>
{
    public CreateFooCommandValidator(IFooValidator fooValidator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            .Must(_ => MustBeAValidFoo())
            .WithMessage("Not a OK Foo!");

        bool MustBeAValidFoo()
            => fooValidator.FooIsOk();
    }
}
