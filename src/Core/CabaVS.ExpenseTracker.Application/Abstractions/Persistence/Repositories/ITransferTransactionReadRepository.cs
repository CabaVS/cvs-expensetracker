using CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ITransferTransactionReadRepository
{
    Task<TransferTransactionModel?> GetByIdAsync(
        Guid workspaceId,
        Guid transferTransactionId,
        CancellationToken cancellationToken = default);
}