using CabaVS.ExpenseTracker.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            options =>
            {
                options.Lifetime = ServiceLifetime.Scoped;
                options.RegisterServicesFromAssembly(AssemblyMarker.Assembly);
            });
        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserOwningRequestBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(WorkspaceBoundedRequestBehavior<,>));
        
        return services;
    }
}
