using Equinor.ProCoSys.PCS5.Command.FooCommands.OverwriteExistingFooAttachment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForOverwriteExistingFooAttachmentCommandTests : AccessValidatorForIFooCommandTests<OverwriteExistingFooAttachmentCommand>
{
    protected override OverwriteExistingFooAttachmentCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, null!, null!, null!);

    protected override OverwriteExistingFooAttachmentCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, null!, null!, null!);
}
