using System.ComponentModel.DataAnnotations;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers;

public class UpdateLinkDto
{
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Url { get; set; } = null!;
    [Required]
    public string RowVersion { get; set; } = null!;
}
