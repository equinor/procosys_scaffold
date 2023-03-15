using Equinor.ProCoSys.PCS5.Domain.AggregateModels.HistoryAggregate;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public EventType EventType { get; set; }
    }
}
