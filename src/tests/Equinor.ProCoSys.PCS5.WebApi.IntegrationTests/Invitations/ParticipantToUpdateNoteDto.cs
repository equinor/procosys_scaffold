namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Invitations
{
    public class ParticipantToUpdateNoteDto
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public string RowVersion { get; set; }
    }
}
