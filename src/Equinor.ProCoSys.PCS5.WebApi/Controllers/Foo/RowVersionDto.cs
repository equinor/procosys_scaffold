using System.ComponentModel.DataAnnotations;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo;

public class RowVersionDto
{
    [Required]
    public string RowVersion { get; set; } = null!;
}
