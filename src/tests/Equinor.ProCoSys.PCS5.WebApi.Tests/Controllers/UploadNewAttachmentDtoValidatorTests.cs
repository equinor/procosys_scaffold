using Equinor.ProCoSys.PCS5.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Controllers;

[TestClass]
public class UploadNewAttachmentDtoValidatorTests : UploadBaseDtoValidatorTests<UploadNewAttachmentDto>
{
    protected override void SetupDut() =>
        _dut = new UploadNewAttachmentDtoValidator<UploadNewAttachmentDto>(_blobStorageOptionsMock.Object);

    protected override UploadNewAttachmentDto GetValidDto() =>
        new()
        {
            File = new TestableFormFile("picture.gif", 1000)
        };
}
