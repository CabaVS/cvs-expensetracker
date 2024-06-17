using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IIncomeTransactionWriteRepository
{
    Task<Guid> Create(IncomeTransaction incomeTransaction, Guid workspaceId, CancellationToken ct = default);
}