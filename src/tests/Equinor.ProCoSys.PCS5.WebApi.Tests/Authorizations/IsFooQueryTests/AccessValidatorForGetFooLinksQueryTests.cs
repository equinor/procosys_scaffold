using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooLinks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooLinksQueryTests : AccessValidatorForIFooQueryTests<GetFooLinksQuery>
{
    protected override GetFooLinksQuery GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject);

    protected override GetFooLinksQuery GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject);
}
