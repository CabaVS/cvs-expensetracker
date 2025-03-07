using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        return services;
    }
}