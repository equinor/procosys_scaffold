using System.ComponentModel.DataAnnotations;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers;

public class CreateLinkDto
{
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Url { get; set; } = null!;
}
