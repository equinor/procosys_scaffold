namespace Equinor.ProCoSys.PCS5.WebApi.Authorizations;

public interface IProjectAccessChecker
{
    bool HasCurrentUserAccessToProject(string projectName);
}