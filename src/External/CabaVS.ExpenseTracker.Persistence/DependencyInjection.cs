using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Persistence;

#pragma warning disable CA1724 // Type names should not match namespaces
public static class DependencyInjection
#pragma warning restore CA1724 // Type names should not match namespaces
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlDatabase");
        
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(connectionString),
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IBalanceQueryRepository, BalanceQueryRepository>();
        services.AddTransient<ICategoryQueryRepository, CategoryQueryRepository>();
        services.AddTransient<ICurrencyQueryRepository, CurrencyQueryRepository>();
        services.AddTransient<ITransactionQueryRepository, TransactionQueryRepository>();
        services.AddTransient<IUserQueryRepository, UserQueryRepository>();
        services.AddTransient<IWorkspaceQueryRepository, WorkspaceQueryRepository>();

        return services;
    }
}
