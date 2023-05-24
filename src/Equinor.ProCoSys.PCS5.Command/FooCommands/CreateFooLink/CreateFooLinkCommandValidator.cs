using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooLink;

public class CreateFooLinkCommandValidator : AbstractValidator<CreateFooLinkCommand>
{
    // todo create unit test
    public CreateFooLinkCommandValidator(IFooValidator fooValidator)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            .MustAsync((command, cancellationToken) => BeAnExistingFoo(command.FooGuid, cancellationToken))
            .WithMessage(command => $"Foo with this ID does not exist! Id={command.FooGuid}")
            .MustAsync((command, cancellationToken) => NotBeAVoidedFoo(command.FooGuid, cancellationToken))
            .WithMessage("Foo is voided!");

        async Task<bool> NotBeAVoidedFoo(Guid fooGuid, CancellationToken cancellationToken)
            => !await fooValidator.FooIsVoidedAsync(fooGuid, cancellationToken);

        async Task<bool> BeAnExistingFoo(Guid fooGuid, CancellationToken cancellationToken)
            => await fooValidator.FooExistsAsync(fooGuid, cancellationToken);
    }
}
