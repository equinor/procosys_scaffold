using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;

public class CreateFooCommandValidator : AbstractValidator<CreateFooCommand>
{
    public CreateFooCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            .Must(_ => MustBeAValidFoo())
            .WithMessage("Not a OK Foo!");

        // some validation can go here
        bool MustBeAValidFoo() => true;
    }
}
