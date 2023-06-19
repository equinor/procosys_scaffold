using Equinor.ProCoSys.PCS5.Command.FooCommands.UpdateFoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForUpdateFooCommandTests : AccessValidatorForIIsFooCommandTests<UpdateFooCommand>
{
    protected override UpdateFooCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, null!, null!, null!);

    protected override UpdateFooCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, null!, null!, null!);
}
