using System.ComponentModel.DataAnnotations;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo;

public class CreateFooDto
{
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string ProjectName { get; set; } = null!;
}
