using System.Collections.Generic;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations
{
    public class AcceptPunchOutDto
    {
        public string InvitationRowVersion { get; set; }
        public string ParticipantRowVersion { get; set; }
        public IEnumerable<ParticipantToUpdateNoteDto> Participants { get; set; }
    }
}
