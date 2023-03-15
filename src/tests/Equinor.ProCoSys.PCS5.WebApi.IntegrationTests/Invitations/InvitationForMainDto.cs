using System;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations
{
    public class InvitationForMainDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DisciplineType Type { get; set; }
        public IpoStatus Status { get; set; }
        public DateTime? CompletedAtUtc { get; set; }
        public DateTime? AcceptedAtUtc { get; set; }
        public DateTime MeetingTimeUtc { get; set; }
        public string RowVersion { get; set; }
    }
}
