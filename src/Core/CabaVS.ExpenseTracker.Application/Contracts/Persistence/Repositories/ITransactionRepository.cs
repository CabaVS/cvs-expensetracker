using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface ITransactionRepository
{
    Task<Guid> AddAsync(Transaction transaction, Guid workspaceId, CancellationToken cancellationToken = default);
}
