using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CabaVS.ExpenseTracker.ArchitectureTests;

internal static class ArchitectureProvider
{
    internal static readonly Architecture Architecture =
        new ArchLoader()
            .LoadAssemblies(
                Domain.AssemblyMarker.Assembly,
                Application.AssemblyMarker.Assembly,
                Persistence.AssemblyMarker.Assembly,
                Infrastructure.AssemblyMarker.Assembly,
                Presentation.AssemblyMarker.Assembly)
            .Build();
    
    internal static readonly IObjectProvider<IType> DomainLayer =
        Types().That().ResideInAssembly(Domain.AssemblyMarker.Assembly).As("Domain Layer");
    internal static readonly IObjectProvider<IType> ApplicationLayer =
        Types().That().ResideInAssembly(Application.AssemblyMarker.Assembly).As("Application Layer");
    internal static readonly IObjectProvider<IType> PersistenceLayer =
        Types().That().ResideInAssembly(Persistence.AssemblyMarker.Assembly).As("Persistence Layer");
    internal static readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInAssembly(Infrastructure.AssemblyMarker.Assembly).As("Infrastructure Layer");
    internal static readonly IObjectProvider<IType> PresentationLayer =
        Types().That().ResideInAssembly(Presentation.AssemblyMarker.Assembly).As("Presentation Layer");
}
