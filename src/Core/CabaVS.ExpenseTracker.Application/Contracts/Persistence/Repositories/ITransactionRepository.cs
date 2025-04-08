using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid transactionId, Guid workspaceId, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Transaction transaction, Guid workspaceId, CancellationToken cancellationToken = default);
    Task RemoveAsync(Transaction transaction, CancellationToken cancellationToken = default);
}
