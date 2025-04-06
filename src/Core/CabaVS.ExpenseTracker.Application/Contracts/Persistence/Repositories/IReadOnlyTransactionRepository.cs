using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IReadOnlyTransactionRepository
{
    Task<TransactionModel[]> GetMultipleAsync(
        Guid workspaceId, DateOnly from, DateOnly to,
        CancellationToken cancellationToken = default);
}
