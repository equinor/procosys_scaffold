using System.Collections.Generic;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations.EditInvitation
{
    public class EditFunctionalRoleDto
    {
        public string Code { get; set; }
        public IList<EditInvitedPersonDto> Persons { get; set; }
        public int Id { get; set; }
        public string RowVersion { get; set; }
    }
}
