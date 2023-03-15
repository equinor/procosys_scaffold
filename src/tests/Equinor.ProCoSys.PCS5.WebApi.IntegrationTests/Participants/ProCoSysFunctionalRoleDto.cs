using System.Collections.Generic;
using Equinor.ProCoSys.PCS5.ForeignApi;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Participants
{
    public class ProCoSysFunctionalRoleDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string InformationEmail { get; set; }
        public bool? UsePersonalEmail { get; set; }
        public IEnumerable<ProCoSysPerson> Persons { get; set; }
    }
}
