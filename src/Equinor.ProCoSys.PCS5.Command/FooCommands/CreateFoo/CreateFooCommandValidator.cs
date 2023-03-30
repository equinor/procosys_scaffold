using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;

public class CreateFooCommandValidator : AbstractValidator<CreateFooCommand>
{
    public CreateFooCommandValidator(IFooValidator fooValidator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            //input validators
            .Must(command =>
                command.ProjectName != null && 
                command.ProjectName.Length >= Foo.ProjectNameMinLength &&
                command.ProjectName.Length < Foo.ProjectNameMaxLength)
            .WithMessage(command =>
                $"Project name must be between {Foo.ProjectNameMinLength} and {Foo.ProjectNameMaxLength} characters! ProjectName={command.ProjectName}")
            .Must(command => 
                command.Title != null &&
                command.Title.Length >= Foo.TitleMinLength && 
                command.Title.Length < Foo.TitleMaxLength)
            .WithMessage(command =>
                $"Title must be between {Foo.TitleMinLength} and {Foo.TitleMaxLength} characters! Title={command.Title}")
            //business validators
            .Must(_ => MustBeAValidFoo())
            .WithMessage("Not a OK Foo!");

        bool MustBeAValidFoo()
            => fooValidator.FooIsOk();
    }
}