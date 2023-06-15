using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Links;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFooLink;

public class UpdateFooLinkCommandValidator : AbstractValidator<UpdateFooLinkCommand>
{
    public UpdateFooLinkCommandValidator(
        IProjectValidator projectValidator,
        IFooValidator fooValidator,
        ILinkService linkService)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            .MustAsync((command, cancellationToken) => NotBeAClosedProjectForFooAsync(command.FooGuid, cancellationToken))
            .WithMessage("Project is closed!")
            .MustAsync((command, cancellationToken) => BeAnExistingFoo(command.FooGuid, cancellationToken))
            .WithMessage(command => $"Foo with this guid does not exist! Guid={command.FooGuid}")
            .MustAsync((command, _) => BeAnExistingLink(command.LinkGuid))
            .WithMessage(command => $"Link with this guid does not exist! Guid={command.LinkGuid}")
            .MustAsync((command, cancellationToken) => NotBeAVoidedFoo(command.FooGuid, cancellationToken))
            .WithMessage("Foo is voided!");

        async Task<bool> NotBeAClosedProjectForFooAsync(Guid fooGuid, CancellationToken cancellationToken)
            => !await projectValidator.IsClosedForFoo(fooGuid, cancellationToken);

        async Task<bool> NotBeAVoidedFoo(Guid fooGuid, CancellationToken cancellationToken)
            => !await fooValidator.FooIsVoidedAsync(fooGuid, cancellationToken);

        async Task<bool> BeAnExistingFoo(Guid fooGuid, CancellationToken cancellationToken)
            => await fooValidator.FooExistsAsync(fooGuid, cancellationToken);

        async Task<bool> BeAnExistingLink(Guid linkGuid)
            => await linkService.ExistsAsync(linkGuid);
    }
}
