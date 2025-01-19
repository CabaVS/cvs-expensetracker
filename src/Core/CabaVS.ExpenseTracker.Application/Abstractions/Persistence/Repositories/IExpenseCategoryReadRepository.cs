using CabaVS.ExpenseTracker.Application.Features.ExpenseCategories.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IExpenseCategoryReadRepository
{
    Task<ExpenseCategoryModel[]> GetAll(Guid workspaceId, CancellationToken cancellationToken);
    Task<ExpenseCategoryModel?> GetById(Guid workspaceId, Guid expenseCategoryId, CancellationToken cancellationToken);
}
