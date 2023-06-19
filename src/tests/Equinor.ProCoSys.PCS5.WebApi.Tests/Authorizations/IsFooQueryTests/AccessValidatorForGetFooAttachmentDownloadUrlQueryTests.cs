using System;
using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooAttachmentDownloadUrl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooAttachmentDownloadUrlQueryTests : AccessValidatorForIIsFooQueryTests<GetFooAttachmentDownloadUrlQuery>
{
    protected override GetFooAttachmentDownloadUrlQuery GetFooQueryWithAccessToProject()
        => new(FooGuidWithAccessToProject, Guid.Empty);

    protected override GetFooAttachmentDownloadUrlQuery GetFooQueryWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, Guid.Empty);
}
