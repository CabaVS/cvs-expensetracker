using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken ct = default);

    ICurrencyRepository BuildCurrencyRepository();
    IWorkspaceRepository BuildWorkspaceRepository();
    IBalanceRepository BuildBalanceRepository();
    IExpenseCategoryRepository BuildExpenseCategoryRepository();
    IIncomeCategoryRepository BuildIncomeCategoryRepository();
    IExpenseTransactionRepository BuildExpenseTransactionRepository();
    IIncomeTransactionRepository BuildIncomeTransactionRepository();
}