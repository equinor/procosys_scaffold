using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo;

public class DeleteFooCommandValidator : AbstractValidator<DeleteFooCommand>
{
    public DeleteFooCommandValidator(
        IProjectValidator projectValidator,
        IFooValidator fooValidator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            .MustAsync((command, cancellationToken) => NotBeAClosedProjectForFooAsync(command.FooGuid, cancellationToken))
            .WithMessage("Project is closed!")
            .MustAsync((command, cancellationToken) => BeAnExistingFoo(command.FooGuid, cancellationToken))
            .WithMessage(command => $"Foo with this guid does not exist! Guid={command.FooGuid}")
            .MustAsync((command, cancellationToken) => BeAVoidedFoo(command.FooGuid, cancellationToken))
            .WithMessage("Foo must be voided before delete!");

        async Task<bool> NotBeAClosedProjectForFooAsync(Guid fooGuid, CancellationToken cancellationToken)
            => !await projectValidator.IsClosedForFoo(fooGuid, cancellationToken);

        async Task<bool> BeAnExistingFoo(Guid fooGuid, CancellationToken cancellationToken)
            => await fooValidator.FooExistsAsync(fooGuid, cancellationToken);

        async Task<bool> BeAVoidedFoo(Guid fooGuid, CancellationToken cancellationToken)
            => await fooValidator.FooIsVoidedAsync(fooGuid, cancellationToken);
    }
}
