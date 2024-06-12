using CabaVS.ExpenseTracker.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(
            configuration => configuration.RegisterServicesFromAssembly(AssemblyMarker.Assembly));

        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(AdminOnlyRequestBehavior<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(WorkspaceBoundedRequestBehavior<,>));

        return serviceCollection;
    }
}