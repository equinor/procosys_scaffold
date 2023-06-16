using System;
using Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFooLink;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForUpdateFooLinkCommandTests : AccessValidatorForIFooCommandTests<UpdateFooLinkCommand>
{
    protected override UpdateFooLinkCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, Guid.Empty, null!, null!, null!);

    protected override UpdateFooLinkCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, Guid.Empty, null!, null!, null!);
}
