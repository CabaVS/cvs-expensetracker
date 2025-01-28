using CabaVS.ExpenseTracker.Application.Features.Categories.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICategoryQueryRepository
{
    Task<CategoryModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<CategoryModel?> GetByIdAsync(Guid workspaceId, Guid categoryId, CancellationToken cancellationToken = default);
}
