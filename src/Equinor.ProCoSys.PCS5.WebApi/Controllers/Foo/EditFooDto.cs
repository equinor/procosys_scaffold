using System.ComponentModel.DataAnnotations;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo;

public class EditFooDto
{
    [Required]
    public string Title { get; set; } = null!;
    public string? Text { get; set; }
    [Required]
    public string RowVersion { get; set; } = null!;
}
