using System;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations
{
    public class CommPkgWithMdpIposDto
    {
        public string CommPkgNo { get; set; }
        public int LatestMdpInvitationId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public bool IsAccepted { get; set; }
    }
}
