using System;
using Equinor.ProCoSys.PCS5.Query.FooQueries.GetFooAttachmentDownloadUrl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;

[TestClass]
public class AccessValidatorForGetFooAttachmentDownloadUrlQueryTests : AccessValidatorForIFooQueryTests<GetFooAttachmentDownloadUrlQuery>
{
    protected override GetFooAttachmentDownloadUrlQuery GetFooCommandWithAccessToProject()
        => new(FooGuidWithAccessToProject, Guid.Empty);

    protected override GetFooAttachmentDownloadUrlQuery GetFooCommandWithoutAccessToProject()
        => new(FooGuidWithoutAccessToProject, Guid.Empty);
}
