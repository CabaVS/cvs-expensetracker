using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    IUserRepository UserRepository { get; }
    IWorkspaceRepository WorkspaceRepository { get; }
    ICurrencyRepository CurrencyRepository { get; }
    IBalanceRepository BalanceRepository { get; }
}
