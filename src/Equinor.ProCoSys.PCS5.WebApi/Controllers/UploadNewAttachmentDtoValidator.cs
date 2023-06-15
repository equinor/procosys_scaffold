using Equinor.ProCoSys.BlobStorage;
using Microsoft.Extensions.Options;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers;

public class UploadNewAttachmentDtoValidator : UploadBaseDtoValidator<UploadNewAttachmentDto>
{
    public UploadNewAttachmentDtoValidator(IOptionsSnapshot<BlobStorageOptions> blobStorageOptions)
    : base(blobStorageOptions)
    {
    }
}
