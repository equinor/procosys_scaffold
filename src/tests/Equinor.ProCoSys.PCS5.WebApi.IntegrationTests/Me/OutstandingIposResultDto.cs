using System.Collections.Generic;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Me
{
    public class OutstandingIposResultDto
    {
        public IEnumerable<OutstandingIpoDetailsDto> Items { get; set; }
    }
}
