using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Application.Interfaces;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFooLink;

public class DeleteFooLinkCommandValidator : AbstractValidator<DeleteFooLinkCommand>
{
    // todo create unit test
    public DeleteFooLinkCommandValidator(IFooValidator fooValidator, ILinkService linkService)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            .MustAsync((command, cancellationToken) => BeAnExistingFoo(command.FooGuid, cancellationToken))
            .WithMessage(command => $"Foo with this guid does not exist! Guid={command.FooGuid}")
            .MustAsync((command, cancellationToken) => BeAnExistingLink(command.LinkGuid, cancellationToken))
            .WithMessage(command => $"Link with this guid does not exist! Guid={command.LinkGuid}")
            .MustAsync((command, cancellationToken) => NotBeAVoidedFoo(command.FooGuid, cancellationToken))
            .WithMessage("Foo is voided!");

        async Task<bool> NotBeAVoidedFoo(Guid fooGuid, CancellationToken cancellationToken)
            => !await fooValidator.FooIsVoidedAsync(fooGuid, cancellationToken);

        async Task<bool> BeAnExistingFoo(Guid fooGuid, CancellationToken cancellationToken)
            => await fooValidator.FooExistsAsync(fooGuid, cancellationToken);

        async Task<bool> BeAnExistingLink(Guid linkGuid, CancellationToken cancellationToken)
            => await linkService.ExistsAsync(linkGuid, cancellationToken);
    }
}
