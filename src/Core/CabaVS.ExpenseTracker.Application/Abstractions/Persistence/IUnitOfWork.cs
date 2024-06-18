using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken ct = default);
    
    ICurrencyRepository CurrencyRepository { get; }
    IWorkspaceRepository WorkspaceRepository { get; }
    IBalanceRepository BalanceRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ITransactionRepository TransactionRepository { get; }
}