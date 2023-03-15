using System.Collections.Generic;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations.CreateInvitation
{
    public class CreateFunctionalRoleDto
    {
        public string Code { get; set; }
        public IList<CreateInvitedPersonDto> Persons { get; set; }
    }
}
