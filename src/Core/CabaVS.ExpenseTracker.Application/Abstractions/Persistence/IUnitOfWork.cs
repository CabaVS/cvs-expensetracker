using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken ct = default);
    
    ICurrencyRepository CurrencyRepository { get; }
    IWorkspaceRepository WorkspaceRepository { get; }
    IBalanceRepository BalanceRepository { get; }
    IExpenseCategoryRepository ExpenseCategoryRepository { get; }
    IIncomeCategoryRepository IncomeCategoryRepository { get; }
    IIncomeTransactionRepository IncomeTransactionRepository { get; }
}