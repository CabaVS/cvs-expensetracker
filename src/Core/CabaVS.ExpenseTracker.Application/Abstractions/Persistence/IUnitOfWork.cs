using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    IGenericCommandRepository<User> UserRepository { get; }
    IGenericCommandRepository<Currency> CurrencyRepository { get; }
    IWorkspaceCommandRepository WorkspaceRepository { get; }
    IGenericCommandRepository<Balance> BalanceRepository { get; }
    IGenericCommandRepository<Category> CategoryRepository { get; }
    IGenericCommandRepository<Transaction> TransactionRepository { get; }
}
