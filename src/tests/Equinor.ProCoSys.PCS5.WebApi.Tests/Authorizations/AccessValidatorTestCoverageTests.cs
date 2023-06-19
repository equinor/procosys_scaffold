using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Equinor.ProCoSys.PCS5.Command;
using Equinor.ProCoSys.PCS5.Query.FooQueries;
using Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsProjectCommandTests;
using Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooCommandTests;
using Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations.IsFooQueryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Equinor.ProCoSys.PCS5.WebApi.Tests.Authorizations;

/// <summary>
/// This unit test don't test any business logic.
/// Just a helper for developer to remember to add Unit Test for AccessValidator when implementing
/// a new IIsProjectCommand, IIsFooCommand or IIsFooQuery
/// </summary>
[TestClass]
public class AccessValidatorTestCoverageTests
{
    [TestMethod]
    public void Each_IIsProjectCommand_ShouldHaveUnitTest_ForAccessValidator()
    {
        var classes =
            GetClassesImplementingInterface(
                "Equinor.ProCoSys.PCS5.Command",
        typeof(IIsProjectCommand));
        var testClasses
            = GetTestsWhichInheritsBaseClass(typeof(AccessValidatorForIIsProjectCommandTests<>));

        AssertAllClassesHasUnitTest(classes, testClasses);
    }

    [TestMethod]
    public void Each_IIsFooCommand_ShouldHaveUnitTest_ForAccessValidator()
    {
        var classes =
            GetClassesImplementingInterface(
                "Equinor.ProCoSys.PCS5.Command",
                typeof(IIsFooCommand));

        var testClasses
            = GetTestsWhichInheritsBaseClass(typeof(AccessValidatorForIIsFooCommandTests<>));

        AssertAllClassesHasUnitTest(classes, testClasses);
    }

    [TestMethod]
    public void Each_IIsFooQuery_ShouldHaveUnitTest_ForAccessValidator()
    {
        var classes =
            GetClassesImplementingInterface(
                "Equinor.ProCoSys.PCS5.Query",
                typeof(IIsFooQuery));

        var testClasses
            = GetTestsWhichInheritsBaseClass(typeof(AccessValidatorForIIsFooQueryTests<>));

        AssertAllClassesHasUnitTest(classes, testClasses);
    }

    private static void AssertAllClassesHasUnitTest(List<Type> classes, List<Type> testClasses)
    {
        Assert.AreNotEqual(0, classes.Count);
        
        var missingTests = new List<string>();
        var genericArguments = GetGenericArgumentsOfBaseClasses(testClasses);
        foreach (var c in classes)
        {
            var classHasTest = genericArguments.Any(g => g.FullName == c.FullName);
            if (!classHasTest)
            {
                missingTests.Add(c.FullName);
            }
        }

        Assert.AreEqual(0, missingTests.Count,
            $"{missingTests.Count} tests are missing: {string.Join(",", missingTests)}");
    }

    private static List<Type> GetGenericArgumentsOfBaseClasses(List<Type> testClasses) =>
        testClasses
            .Where(a => a.BaseType != null)
            .SelectMany(a => a.BaseType.GetGenericArguments()).ToList();

    private static List<Type> GetTestsWhichInheritsBaseClass(Type baseClass)
    {
        var baseClassFullName = baseClass.FullName;
        Assert.IsNotNull(baseClassFullName);

        var testAssembly = Assembly.GetExecutingAssembly();
        var accessValidatorForIFooQueryTestClasses =
            testAssembly.GetTypes()
                .Where(t =>
                    IsAEquinorClass(t) &&
                    IsATestClass(t) &&
                    HasBaseClassOfType(t, baseClassFullName))
                .ToList();
        return accessValidatorForIFooQueryTestClasses;
    }

    private static bool IsAEquinorClass(Type type)
    {
        var isAEquinorClass =
            type.FullName != null &&
            type.FullName.Contains("Equinor.");
        return isAEquinorClass;
    }

    private static bool HasBaseClassOfType(Type type, string baseClassFullName)
    {
        var hasBaseClassOfType =
            type.BaseType != null &&
            type.BaseType.FullName != null &&
            type.BaseType.FullName.StartsWith(baseClassFullName);

        return hasBaseClassOfType;
    }

    private static bool IsATestClass(Type type)
    {
        var customAttributes = type.CustomAttributes;
        var isATestClass =
            customAttributes.Any(a => a.AttributeType == typeof(TestClassAttribute)) &&
            type.IsClass &&
            !type.IsAbstract;
        return isATestClass;
    }

    private static List<Type> GetClassesImplementingInterface(
        string assemblyName,
        Type interfaceType)
    {
        var assemblyWithClasses =
            AppDomain.CurrentDomain.GetAssemblies()
                .Single(a => a.FullName != null &&
                             a.FullName.Contains(assemblyName) &&
                             !a.FullName.Contains(".Test"));

        var classes =
            assemblyWithClasses.GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ToList();
        return classes;
    }
}
