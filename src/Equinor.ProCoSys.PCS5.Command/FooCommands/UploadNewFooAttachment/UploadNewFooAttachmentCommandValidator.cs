using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.ProjectValidators;
using FluentValidation;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.UploadNewFooAttachment;

public class UploadNewFooAttachmentCommandValidator : AbstractValidator<UploadNewFooAttachmentCommand>
{
    public UploadNewFooAttachmentCommandValidator(
        IProjectValidator projectValidator,
        IFooValidator fooValidator,
        IAttachmentService attachmentService)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command)
            .MustAsync((command, cancellationToken) => NotBeAClosedProjectForFooAsync(command.FooGuid, cancellationToken))
            .WithMessage("Project is closed!")
            .MustAsync((command, cancellationToken) => BeAnExistingFoo(command.FooGuid, cancellationToken))
            .WithMessage(command => $"Foo with this guid does not exist! Guid={command.FooGuid}")
            .MustAsync((command, cancellationToken) => NotBeAVoidedFoo(command.FooGuid, cancellationToken))
            .WithMessage("Foo is voided!")
            .MustAsync((command, _) => NotHaveAttachmentWithFilenameAsync(command.FooGuid, command.FileName))
            .WithMessage(command => $"Foo already has an attachment with filename {command.FileName}! Please rename file or choose to overwrite");

        async Task<bool> NotBeAClosedProjectForFooAsync(Guid fooGuid, CancellationToken cancellationToken)
            => !await projectValidator.IsClosedForFoo(fooGuid, cancellationToken);

        async Task<bool> NotBeAVoidedFoo(Guid fooGuid, CancellationToken cancellationToken)
            => !await fooValidator.FooIsVoidedAsync(fooGuid, cancellationToken);

        async Task<bool> BeAnExistingFoo(Guid fooGuid, CancellationToken cancellationToken)
            => await fooValidator.FooExistsAsync(fooGuid, cancellationToken);

        async Task<bool> NotHaveAttachmentWithFilenameAsync(Guid fooGuid, string fileName)
            => !await attachmentService.FilenameExistsForSourceAsync(fooGuid, fileName);
    }
}
