using System.ComponentModel.DataAnnotations;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers;

public class OverwriteAttachmentDto : UploadBaseDto
{
    [Required]
    public string RowVersion { get; set; } = null!;
}
