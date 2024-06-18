using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IExpenseTransactionRepository
{
    Task<Guid> Create(ExpenseTransaction expenseTransaction, Guid workspaceId, CancellationToken ct = default);
}