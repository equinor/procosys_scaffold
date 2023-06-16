using Equinor.ProCoSys.PCS5.Command.FooCommands.UploadNewFooAttachment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForUploadNewFooAttachmentCommandTests : AccessValidatorForIFooCommandTests<UploadNewFooAttachmentCommand>
{
    protected override UploadNewFooAttachmentCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, null!, null!);

    protected override UploadNewFooAttachmentCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, null!, null!);
}
