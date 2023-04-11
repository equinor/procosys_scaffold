namespace Equinor.ProCoSys.PCS5.WebApi.Controllers;

public interface IRowVersionValidator
{
    bool IsValid(string rowVersion);
}
