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
        
        return services;
    }
}