using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken cancellationToken = default);
    
    IUserRepository UserRepository { get; }
    ICurrencyRepository CurrencyRepository { get; }
    IWorkspaceRepository WorkspaceRepository { get; }
    IBalanceRepository BalanceRepository { get; }
    IExpenseCategoryRepository ExpenseCategoryRepository { get; }
    ITransferTransactionRepository TransferTransactionRepository { get; }
}
