using CabaVS.ExpenseTracker.Application.Features.Balances.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IBalanceReadRepository
{
    Task<BalanceModel[]> GetBalances(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<BalanceModel?> GetBalanceById(Guid balanceId, Guid workspaceId, CancellationToken cancellationToken = default);
}