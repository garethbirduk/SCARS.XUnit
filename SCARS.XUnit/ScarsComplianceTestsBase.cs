using SCARS.ArchitectureRules;
using System.Reflection;
using Xunit;

namespace SCARS.XUnit;

public abstract class ScarsComplianceTestsBase
{
    protected abstract Assembly TargetAssembly { get; }
    protected abstract Assembly TestAssembly { get; }

    [Fact]
    public void ScarsClassRuleAttributes_ShouldNotBeViolated()
    {
        var analyzer = new Analyzer();
        var violations = analyzer.GetScarsClassRuleAttributeViolations(TargetAssembly).ToList();

        Assert.True(violations.Count == 0, $"SCARS violations found:\n{string.Join("\n", violations.Select(v => $"- {v.Type.Name} violated rule: {v.Rule.Description}"))}");
    }

    [Fact]
    public void UnmockableServices_ShouldNotBeMocked()
    {
        var violations = UnmockableRule.FindViolations(TestAssembly).ToList();

        Assert.True(violations.Count == 0, string.Join("\n",
            violations.Select(v => $"❌ {v.TestClass.Name} mocks unmockable type {v.MockedType.Name}")));
    }
}