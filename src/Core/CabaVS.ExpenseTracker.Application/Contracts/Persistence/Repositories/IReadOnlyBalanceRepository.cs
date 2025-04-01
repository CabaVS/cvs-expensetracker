using CabaVS.ExpenseTracker.Application.Features.Balances.Models;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IReadOnlyBalanceRepository
{
    Task<BalanceModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken);
    Task<BalanceDetailsModel?> GetDetailsAsync(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken);
}
