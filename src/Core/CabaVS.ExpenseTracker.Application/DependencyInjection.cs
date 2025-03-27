using CabaVS.ExpenseTracker.Application.Common;
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
        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserOwningRequestBehavior<,>));
        
        return services;
    }
}
