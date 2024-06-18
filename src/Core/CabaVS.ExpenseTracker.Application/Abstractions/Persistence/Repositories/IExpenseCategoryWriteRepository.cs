using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IExpenseCategoryWriteRepository
{
    Task<Guid> Create(ExpenseCategory expenseCategory, Guid workspaceId, CancellationToken ct = default);
}