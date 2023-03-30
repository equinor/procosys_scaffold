namespace Equinor.ProCoSys.PCS5.Command;

public class IdAndRowVersion
{
    public IdAndRowVersion(int id, string rowVersion)
    {
        Id = id;
        RowVersion = rowVersion;
    }

    public int Id { get; }
    public string RowVersion { get; }
}