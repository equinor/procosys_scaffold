using System.Collections.Generic;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Scope
{
    public class ProCoSysCommPkgSearchDto
    {
        public int MaxAvailable { get; set; }
        public IList<ProCoSysCommPkgDto> CommPkgs { get; set; }
    }
}
