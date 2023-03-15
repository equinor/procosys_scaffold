namespace Equinor.ProCoSys.PCS5.Command.Validators.RowVersionValidators
{
    public interface IRowVersionValidator
    {
        bool IsValid(string rowVersion);
    }
}
