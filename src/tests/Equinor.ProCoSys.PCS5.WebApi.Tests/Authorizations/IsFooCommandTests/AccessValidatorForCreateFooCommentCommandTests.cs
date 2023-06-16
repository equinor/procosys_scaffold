using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFooComment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;

[TestClass]
public class AccessValidatorForCreateFooCommentCommandTests : AccessValidatorForIFooCommandTests<CreateFooCommentCommand>
{
    protected override CreateFooCommentCommand GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, null!);

    protected override CreateFooCommentCommand GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, null!);
}
