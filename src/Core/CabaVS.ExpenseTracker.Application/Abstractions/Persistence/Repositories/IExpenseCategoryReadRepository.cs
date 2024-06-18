using CabaVS.ExpenseTracker.Application.Features.Categories.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IExpenseCategoryReadRepository
{
    Task<ExpenseCategoryModel[]> GetAll(Guid workspaceId, CancellationToken ct = default);
    Task<ExpenseCategoryModel?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default);
}