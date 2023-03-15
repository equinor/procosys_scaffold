using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations.EditInvitation
{
    public class EditParticipantDto
    {
        public Organization Organization { get; set; }
        public int SortKey { get; set; }
        public EditExternalEmailDto ExternalEmail { get; set; }
        public EditInvitedPersonDto Person { get; set; }
        public EditFunctionalRoleDto FunctionalRole { get; set; }
    }
}
