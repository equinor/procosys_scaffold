using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooComments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooCommentsQueryTests : AccessValidatorForIIsFooQueryTests<GetFooCommentsQuery>
{
    protected override GetFooCommentsQuery GetFooQueryWithAccessToProject()
        => new(FooGuidWithAccessToProject);

    protected override GetFooCommentsQuery GetFooQueryWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject);
}
