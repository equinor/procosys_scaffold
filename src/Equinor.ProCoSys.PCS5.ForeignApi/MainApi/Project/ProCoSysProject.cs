using System;

namespace Equinor.ProCoSys.PCS5.ForeignApi.MainApi.Project;
#pragma warning disable CS8618
public class ProCoSysProject
{
    public int Id { get; set; }
    public Guid ProCoSysGuid { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsClosed { get; set; }
}