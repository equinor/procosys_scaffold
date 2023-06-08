using System.ComponentModel.DataAnnotations;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers;

public class CreateCommentDto
{
    [Required]
    public string Text { get; set; } = null!;
}
