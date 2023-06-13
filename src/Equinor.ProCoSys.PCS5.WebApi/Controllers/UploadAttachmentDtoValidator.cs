using Equinor.ProCoSys.BlobStorage;
using Microsoft.Extensions.Options;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers;

public class UploadAttachmentDtoValidator<T> : UploadBaseDtoValidator<UploadAttachmentDto>
{
    public UploadAttachmentDtoValidator(IOptionsSnapshot<BlobStorageOptions> blobStorageOptions)
    : base(blobStorageOptions)
    {
    }
}
