using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IExpenseTransactionRepository
{
    Task<ExpenseTransaction?> GetById(Guid expenseTransactionId, Guid workspaceId, CancellationToken ct = default);
    Task<Guid> Create(ExpenseTransaction expenseTransaction, Guid workspaceId, CancellationToken ct = default);
    Task Update(ExpenseTransaction expenseTransaction, Guid workspaceId, CancellationToken ct = default);
    Task Delete(ExpenseTransaction expenseTransaction, Guid workspaceId, CancellationToken ct = default);
}