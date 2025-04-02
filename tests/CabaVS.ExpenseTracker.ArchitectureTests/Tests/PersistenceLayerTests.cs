using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CabaVS.ExpenseTracker.ArchitectureTests.Tests;

public sealed class PersistenceLayerTests
{
    [Fact]
    public void Persistence_ShouldNot_DependOnAnyUpperLayer()
    {
        TypesShouldConjunction rule =
            Types().That().Are(ArchitectureProvider.PersistenceLayer)
            .Should().NotDependOnAny(ArchitectureProvider.InfrastructureLayer)
            .AndShould().NotDependOnAny(ArchitectureProvider.PresentationLayer);
        
        rule.Check(ArchitectureProvider.Architecture);
    }
}
