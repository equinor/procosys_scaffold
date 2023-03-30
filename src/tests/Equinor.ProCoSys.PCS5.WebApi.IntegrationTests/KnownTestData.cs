using System;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests;

public class KnownTestData
{
    public KnownTestData(string plant) => Plant = plant;

    public string Plant { get; }

    public static Guid ProjectProCoSysGuidA => new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static string ProjectNameA => "TestProject A";
    public static string ProjectDescriptionA => "Test - Project A";
    public static string FooA => "FOO-A";
    public static Guid ProjectProCoSysGuidB => new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static string ProjectNameB => "TestProject B";
    public static string ProjectDescriptionB => "Test - Project B";
    public static string FooB => "Foo-B";

    public int FooAId { get; set; }
    public int FooBId { get; set; }
}