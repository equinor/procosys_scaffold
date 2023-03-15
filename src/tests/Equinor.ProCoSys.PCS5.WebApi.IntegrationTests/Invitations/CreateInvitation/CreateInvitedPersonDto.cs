using System;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations.CreateInvitation
{
    public class CreateInvitedPersonDto
    {
        public Guid AzureOid { get; set; }
        public string Email { get; set; }
        public bool Required { get; set; }
    }
}
