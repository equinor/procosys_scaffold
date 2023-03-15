using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Me
{
    public class OutstandingIpoDetailsDto
    {
        public int InvitationId { get; set; }
        public string Description { get; set; }
        public Organization Organization { get; set; }
    }
}
