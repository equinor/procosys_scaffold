using System.Collections.Generic;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations
{
    public class InvitationResultsDto
    {
        public int MaxAvailable { get; set; }
        public IList<Query.GetInvitationsQueries.InvitationForQueryDto> Invitations { get; set; }
}
}
