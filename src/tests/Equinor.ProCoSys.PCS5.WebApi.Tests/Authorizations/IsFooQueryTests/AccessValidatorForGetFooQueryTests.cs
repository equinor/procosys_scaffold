using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooQueryTests : AccessValidatorForIIsFooQueryTests<GetFooQuery>
{
    protected override GetFooQuery GetFooQueryWithAccessToProject()
        => new(FooGuidWithAccessToProject);

    protected override GetFooQuery GetFooQueryWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject);
}
