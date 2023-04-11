using Equinor.ProCoSys.PCS5.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests;

[TestClass]
public abstract class TestBase
{
    private readonly RowVersionValidator _rowVersionValidator = new();
        
    [AssemblyCleanup]
    public static void AssemblyCleanup() => TestFactory.Instance.Dispose();

    public void AssertRowVersionChange(string oldRowVersion, string newRowVersion)
    {
        Assert.IsTrue(IsAValidRowVersion(oldRowVersion));
        Assert.IsTrue(IsAValidRowVersion(newRowVersion));
        Assert.AreNotEqual(oldRowVersion, newRowVersion);
    }

    public bool IsAValidRowVersion(string oldRowVersion) => _rowVersionValidator.IsValid(oldRowVersion);

    protected void AssertCreatedBy(UserType userType, PersonDto personDto)
    {
        var profile = TestFactory.Instance.GetTestProfile(userType);
        AssertUser(profile, personDto);
    }

    protected void AssertUser(TestProfile profile, PersonDto personDto)
    {
        Assert.IsNotNull(personDto);
        Assert.AreEqual(profile.FirstName, personDto.FirstName);
        Assert.AreEqual(profile.LastName, personDto.LastName);
    }
}
