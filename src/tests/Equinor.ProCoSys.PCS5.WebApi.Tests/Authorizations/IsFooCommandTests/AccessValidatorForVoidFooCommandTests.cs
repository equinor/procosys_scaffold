using Equinor.ProCoSys.PCS5.Command.FooCommands.VoidFoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForVoidFooCommandTests : AccessValidatorForIFooCommandTests<VoidFooCommand>
{
    protected override VoidFooCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, null!);

    protected override VoidFooCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, null!);
}
