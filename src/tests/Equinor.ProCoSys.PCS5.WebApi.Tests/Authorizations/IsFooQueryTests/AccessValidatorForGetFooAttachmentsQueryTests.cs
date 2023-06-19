using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooAttachments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooAttachmentsQueryTests : AccessValidatorForIIsFooQueryTests<GetFooAttachmentsQuery>
{
    protected override GetFooAttachmentsQuery GetFooQueryWithAccessToProject()
        => new(FooGuidWithAccessToProject);

    protected override GetFooAttachmentsQuery GetFooQueryWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject);
}
