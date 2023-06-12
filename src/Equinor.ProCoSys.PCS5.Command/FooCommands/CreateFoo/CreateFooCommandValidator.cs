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

        RuleFor(command => command)
            .MustAsync(BeAnExistingProjectAsync)
            .WithMessage(command => $"Project with this name does not exist! Guid={command.ProjectName}")
            .MustAsync(NotBeAClosedProjectAsync)
            .WithMessage("Project is closed!");

        async Task<bool> BeAnExistingProjectAsync(CreateFooCommand command, CancellationToken token)
            => await projectValidator.ExistsAsync(command.ProjectName, token);

        async Task<bool> NotBeAClosedProjectAsync(CreateFooCommand command, CancellationToken token)
            => !await projectValidator.IsClosed(command.ProjectName, token);
    }
}
