using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooComments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooCommentsQueryTests : AccessValidatorForIFooQueryTests<GetFooCommentsQuery>
{
    protected override GetFooCommentsQuery GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject);

    protected override GetFooCommentsQuery GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject);
}
