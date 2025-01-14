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

        services.AddTransient<IUserReadRepository, UserReadRepository>();
        services.AddTransient<IWorkspaceReadRepository, WorkspaceReadRepository>();
        services.AddTransient<ICurrencyReadRepository, CurrencyReadRepository>();
        services.AddTransient<IBalanceReadRepository, BalanceReadRepository>();
        services.AddTransient<ITransferTransactionReadRepository, TransferTransactionReadRepository>();

        return services;
    }
}
