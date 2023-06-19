using Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForDeleteFooCommandTests : AccessValidatorForIIsFooCommandTests<DeleteFooCommand>
{
    protected override DeleteFooCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, null!);

    protected override DeleteFooCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, null!);
}
