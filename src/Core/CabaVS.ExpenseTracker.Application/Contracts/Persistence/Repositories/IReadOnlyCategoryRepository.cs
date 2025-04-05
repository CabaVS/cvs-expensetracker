using CabaVS.ExpenseTracker.Application.Features.Categories.Models;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IReadOnlyCategoryRepository
{
    Task<CategoryModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<CategoryModel?> GetByIdAsync(Guid workspaceId, Guid categoryId, CancellationToken cancellationToken = default);
}
