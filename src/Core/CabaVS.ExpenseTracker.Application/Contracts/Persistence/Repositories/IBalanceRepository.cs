using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IBalanceRepository
{
    Task<Balance?> GetByIdAsync(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Balance balance, Guid workspaceId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Balance balance, Guid workspaceId, CancellationToken cancellationToken = default);
}
