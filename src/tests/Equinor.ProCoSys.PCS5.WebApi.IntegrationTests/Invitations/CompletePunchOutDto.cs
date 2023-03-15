using System.Collections.Generic;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations
{
    public class CompletePunchOutDto
    {
        public string InvitationRowVersion { get; set; }
        public string ParticipantRowVersion { get; set; }
        public IEnumerable<ParticipantToChangeDto> Participants { get; set; }
    }
}
