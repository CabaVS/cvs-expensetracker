using CabaVS.ExpenseTracker.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Application;

#pragma warning disable CA1724 // Type names should not match namespaces
public static class DependencyInjection
#pragma warning restore CA1724 // Type names should not match namespaces
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(
            configuration => configuration.RegisterServicesFromAssembly(AssemblyMarker.Assembly));
        
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthenticatedUserRequestBehavior<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(WorkspaceBoundRequestBehavior<,>));

        return serviceCollection;
    }
}
