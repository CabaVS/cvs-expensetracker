using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IIncomeTransactionRepository
{
    Task<Guid> Create(IncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default);
}