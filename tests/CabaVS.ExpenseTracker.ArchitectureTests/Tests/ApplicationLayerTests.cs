using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CabaVS.ExpenseTracker.ArchitectureTests.Tests;

public sealed class ApplicationLayerTests
{
    [Fact]
    public void Application_ShouldNot_DependOnAnyUpperLayer()
    {
        TypesShouldConjunction rule =
            Types().That().Are(ArchitectureProvider.ApplicationLayer)
            .Should().NotDependOnAny(ArchitectureProvider.PersistenceLayer)
            .AndShould().NotDependOnAny(ArchitectureProvider.InfrastructureLayer)
            .AndShould().NotDependOnAny(ArchitectureProvider.PresentationLayer);
        
        rule.Check(ArchitectureProvider.Architecture);
    }
}
