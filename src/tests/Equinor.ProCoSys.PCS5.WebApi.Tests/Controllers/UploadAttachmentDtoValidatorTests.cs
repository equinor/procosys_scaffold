using Equinor.ProCoSys.PCS5.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Controllers;

[TestClass]
public class UploadAttachmentDtoValidatorTests : UploadBaseDtoValidatorTests<UploadAttachmentDto>
{
    protected override void SetupDut() =>
        _dut = new UploadAttachmentDtoValidator<UploadAttachmentDto>(_blobStorageOptionsMock.Object);

    protected override UploadAttachmentDto GetValidDto() =>
        new()
        {
            File = new TestableFormFile("picture.gif", 1000)
        };
}
