using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooLink;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForCreateFooLinkCommandTests : AccessValidatorForIIsFooCommandTests<CreateFooLinkCommand>
{
    protected override CreateFooLinkCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, null!, null!);

    protected override CreateFooLinkCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, null!, null!);
}
