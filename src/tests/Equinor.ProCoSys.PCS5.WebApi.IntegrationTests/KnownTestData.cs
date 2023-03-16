namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests
{
    public class KnownTestData
    {
        public static string ProjectName => "TestProject";
        public static string ProjectDescription => "Test - Project";
        public static string FooA => "FOO-A";

        public KnownTestData(string plant) => Plant = plant;

        public string Plant { get; }

        public int FooAId { get; set; }
    }
}
