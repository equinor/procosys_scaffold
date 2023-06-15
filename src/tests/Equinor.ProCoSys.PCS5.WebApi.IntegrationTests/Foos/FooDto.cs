using System;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos;

public class FooDto
{
    public Guid Guid { get; set; }
    public string ProjectName { get; set; }
    public string Title { get; set; }
    public bool IsVoided { get; set; }
    public string RowVersion { get; set; }
}
