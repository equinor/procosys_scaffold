using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooAttachments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooAttachmentsQueryTests : AccessValidatorForIFooQueryTests<GetFooAttachmentsQuery>
{
    protected override GetFooAttachmentsQuery GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject);

    protected override GetFooAttachmentsQuery GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject);
}
