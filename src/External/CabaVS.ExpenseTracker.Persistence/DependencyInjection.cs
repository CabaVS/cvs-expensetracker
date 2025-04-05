using CabaVS.ExpenseTracker.Application.Contracts.Persistence;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Dapper;
using CabaVS.ExpenseTracker.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlDatabase")
            ?? throw new InvalidOperationException("Connection string to the database is not configured.");
        
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(connectionString),
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);
        
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(
            _ => new SqlConnectionFactory(connectionString));

        services.AddSingleton<IReadOnlyUserRepository, ReadOnlyUserRepository>();
        services.AddSingleton<IReadOnlyWorkspaceRepository, ReadOnlyWorkspaceRepository>();
        services.AddSingleton<IReadOnlyCurrencyRepository, ReadOnlyCurrencyRepository>();
        services.AddSingleton<IReadOnlyBalanceRepository, ReadOnlyBalanceRepository>();
        services.AddSingleton<IReadOnlyCategoryRepository, ReadOnlyCategoryRepository>();
        
        return services;
    }
}
