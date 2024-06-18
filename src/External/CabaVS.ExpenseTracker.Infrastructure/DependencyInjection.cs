using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Converters;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;
using Dapper;
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

        serviceCollection.AddSingleton<SqlConnectionFactory>();
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        
        serviceCollection.AddSingleton<IUserReadRepository, UserReadRepository>();
        serviceCollection.AddSingleton<IWorkspaceReadRepository, WorkspaceReadRepository>();
        serviceCollection.AddSingleton<ICurrencyReadRepository, CurrencyReadRepository>();
        serviceCollection.AddSingleton<IBalanceReadRepository, BalanceReadRepository>();
        serviceCollection.AddSingleton<IExpenseCategoryReadRepository, ExpenseCategoryReadRepository>();
        serviceCollection.AddSingleton<IIncomeCategoryReadRepository, IncomeCategoryReadRepository>();
        serviceCollection.AddSingleton<IIncomeTransactionReadRepository, IncomeTransactionReadRepository>();
        
        return serviceCollection;
    }
}