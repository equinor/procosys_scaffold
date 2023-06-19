using System;
using Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFooLink;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForDeleteFooLinkCommandTests : AccessValidatorForIIsFooCommandTests<DeleteFooLinkCommand>
{
    protected override DeleteFooLinkCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, Guid.Empty, null!);

    protected override DeleteFooLinkCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, Guid.Empty, null!);
}
