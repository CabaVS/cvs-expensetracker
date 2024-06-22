using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IIncomeTransactionRepository
{
    Task<IncomeTransaction?> GetById(Guid incomeTransactionId, Guid workspaceId, CancellationToken ct = default);
    Task<Guid> Create(IncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default);
    Task Update(IncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default);
    Task Delete(IncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default);
}