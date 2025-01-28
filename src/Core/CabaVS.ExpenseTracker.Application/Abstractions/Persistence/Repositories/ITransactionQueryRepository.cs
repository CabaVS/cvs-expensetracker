using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ITransactionQueryRepository
{
    Task<TransactionModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<TransactionModel?> GetByIdAsync(Guid workspaceId, Guid transactionId, CancellationToken cancellationToken = default);
}
