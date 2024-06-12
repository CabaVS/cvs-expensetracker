using CabaVS.ExpenseTracker.Application.Features.Balances.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IBalanceReadRepository
{
    Task<BalanceModel[]> GetAll(Guid workspaceId, CancellationToken ct = default);
}