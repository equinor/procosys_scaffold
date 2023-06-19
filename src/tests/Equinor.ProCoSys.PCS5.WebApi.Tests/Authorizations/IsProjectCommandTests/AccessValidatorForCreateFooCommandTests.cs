using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsProjectCommandTests;

[TestClass]
public class AccessValidatorForCreateFooCommandTests : AccessValidatorForIIsProjectCommandTests<CreateFooCommand>
{
    protected override CreateFooCommand GetProjectRequestWithAccessToProjectToTest()
        => new(null!, ProjectWithAccess);

    protected override CreateFooCommand GetProjectRequestWithoutAccessToProjectToTest()
        => new(null!, ProjectWithoutAccess);
}
