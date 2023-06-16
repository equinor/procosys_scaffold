using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooQueryTests : AccessValidatorForIFooQueryTests<GetFooQuery>
{
    protected override GetFooQuery GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject);

    protected override GetFooQuery GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject);
}
