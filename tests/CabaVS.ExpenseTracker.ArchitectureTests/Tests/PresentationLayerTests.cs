using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CabaVS.ExpenseTracker.ArchitectureTests.Tests;

public sealed class PresentationLayerTests
{
    [Fact]
    public void Presentation_ShouldNot_DependOnAnyUpperLayer()
    {
        TypesShouldConjunction rule =
            Types().That().Are(ArchitectureProvider.PresentationLayer)
            .Should().NotDependOnAny(ArchitectureProvider.PersistenceLayer)
            .AndShould().NotDependOnAny(ArchitectureProvider.InfrastructureLayer);
        
        rule.Check(ArchitectureProvider.Architecture);
    }
}
