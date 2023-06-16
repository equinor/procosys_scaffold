using System;
using Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFooAttachment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForDeleteFooAttachmentCommandTests : AccessValidatorForIFooCommandTests<DeleteFooAttachmentCommand>
{
    protected override DeleteFooAttachmentCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, Guid.Empty, null!);

    protected override DeleteFooAttachmentCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, Guid.Empty, null!);
}
