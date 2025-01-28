using CabaVS.ExpenseTracker.Application.Features.Balances.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IBalanceQueryRepository
{
    Task<BalanceModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<BalanceModel?> GetByIdAsync(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken = default);
}
