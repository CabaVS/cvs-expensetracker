using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("SqlDatabase")),
            ServiceLifetime.Transient,
            ServiceLifetime.Transient);
        
        serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
        
        serviceCollection.AddTransient<IUserReadRepository, UserReadRepository>();
        serviceCollection.AddTransient<IWorkspaceReadRepository, WorkspaceReadRepository>();
        serviceCollection.AddTransient<ICurrencyReadRepository, CurrencyReadRepository>();
        serviceCollection.AddTransient<IBalanceReadRepository, BalanceReadRepository>();
        serviceCollection.AddTransient<IExpenseCategoryReadRepository, ExpenseCategoryReadRepository>();
        serviceCollection.AddTransient<IIncomeCategoryReadRepository, IncomeCategoryReadRepository>();
        serviceCollection.AddTransient<IExpenseTransactionReadRepository, ExpenseTransactionReadRepository>();
        serviceCollection.AddTransient<IIncomeTransactionReadRepository, IncomeTransactionReadRepository>();
        
        return serviceCollection;
    }
}