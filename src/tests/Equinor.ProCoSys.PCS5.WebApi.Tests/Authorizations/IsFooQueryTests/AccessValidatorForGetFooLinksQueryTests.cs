using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooLinks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooLinksQueryTests : AccessValidatorForIIsFooQueryTests<GetFooLinksQuery>
{
    protected override GetFooLinksQuery GetFooQueryWithAccessToProject()
        => new(FooGuidWithAccessToProject);

    protected override GetFooLinksQuery GetFooQueryWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject);
}
