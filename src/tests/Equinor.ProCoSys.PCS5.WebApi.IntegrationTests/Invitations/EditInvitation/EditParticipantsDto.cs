using System.Collections.Generic;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations.EditInvitation
{
    public class EditParticipantsDto
    {
        public IEnumerable<EditParticipantDto> UpdatedParticipants { get; set; }
    }
}
