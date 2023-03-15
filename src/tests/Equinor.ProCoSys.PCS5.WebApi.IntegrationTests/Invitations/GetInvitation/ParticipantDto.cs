using System;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations.GetInvitation
{
    public class ParticipantDto
    {
        public int Id { get; set; }
        public Organization Organization { get; set; }
        public int SortKey { get; set; }
        public PersonDto SignedBy { get; set; }
        public DateTime? SignedAtUtc { get; set; }
        public string Note { get; set; }
        public bool Attended { get; set; }
        public bool CanSign { get; set; }
        public ExternalEmailDto ExternalEmail { get; set; }
        public InvitedPersonDto Person { get; set; }
        public FunctionalRoleDto FunctionalRole { get; set; }
        public string RowVersion { get; set; }
    }
}
