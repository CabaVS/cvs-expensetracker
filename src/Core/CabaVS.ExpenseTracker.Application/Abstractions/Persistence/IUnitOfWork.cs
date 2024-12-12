using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken cancellationToken = default);
    
    IUserRepository BuildUserRepository();
    ICurrencyRepository BuildCurrencyRepository();
    IWorkspaceRepository BuildWorkspaceRepository();
    IBalanceRepository BuildBalanceRepository();
}