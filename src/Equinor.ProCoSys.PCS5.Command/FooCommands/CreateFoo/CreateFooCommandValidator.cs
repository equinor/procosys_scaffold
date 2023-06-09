using System.Threading.Tasks;
using System.Threading;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;

public class CreateFooCommandValidator : AbstractValidator<CreateFooCommand>
{
    public CreateFooCommandValidator(IProjectValidator projectValidator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        // todo add rule to also check that project exists ... all projects must be synced in front
        RuleFor(command => command)
            .MustAsync(NotBeAClosedProjectAsync)
            .WithMessage("Project is closed!");

        async Task<bool> NotBeAClosedProjectAsync(CreateFooCommand command, CancellationToken token)
            => !await projectValidator.IsClosed(command.ProjectName, token);
    }
}
