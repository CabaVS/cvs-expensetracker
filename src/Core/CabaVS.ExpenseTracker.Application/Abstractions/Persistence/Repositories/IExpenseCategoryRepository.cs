using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IExpenseCategoryRepository
{
    Task<ExpenseCategory?> GetByIdAsync(Guid workspaceId, Guid expenseCategoryId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(Guid workspaceId, ExpenseCategory expenseCategory, CancellationToken cancellationToken);
    Task UpdateAsync(Guid workspaceId, ExpenseCategory expenseCategory, CancellationToken cancellationToken);
    Task DeleteAsync(Guid workspaceId, ExpenseCategory expenseCategory, CancellationToken cancellationToken);
}
