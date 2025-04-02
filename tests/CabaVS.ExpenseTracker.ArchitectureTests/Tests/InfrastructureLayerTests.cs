using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CabaVS.ExpenseTracker.ArchitectureTests.Tests;

public sealed class InfrastructureLayerTests
{
    [Fact]
    public void Infrastructure_ShouldNot_DependOnAnyUpperLayer()
    {
        TypesShouldConjunction rule =
            Types().That().Are(ArchitectureProvider.InfrastructureLayer)
            .Should().NotDependOnAny(ArchitectureProvider.PersistenceLayer)
            .AndShould().NotDependOnAny(ArchitectureProvider.PresentationLayer);
        
        rule.Check(ArchitectureProvider.Architecture);
    }
}
