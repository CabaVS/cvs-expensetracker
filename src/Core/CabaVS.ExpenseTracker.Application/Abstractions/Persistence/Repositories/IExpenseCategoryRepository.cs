using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IExpenseCategoryRepository
{
    Task<ExpenseCategory?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default);
    Task<Guid> Create(ExpenseCategory expenseCategory, Guid workspaceId, CancellationToken ct = default);
    Task Update(ExpenseCategory expenseCategory, Guid workspaceId, CancellationToken ct = default);
    Task Delete(ExpenseCategory expenseCategory, Guid workspaceId, CancellationToken ct = default);
}