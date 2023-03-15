namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations
{
    public class ParticipantToUpdateAttendedStatusDto
    {
        public int Id { get; set; }
        public bool Attended { get; set; }
        public string RowVersion { get; set; }
    }
}
