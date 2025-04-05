using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IBalanceRepository
{
    Task<Guid> CreateAsync(Balance balance, Guid workspaceId, CancellationToken cancellationToken = default);
}
