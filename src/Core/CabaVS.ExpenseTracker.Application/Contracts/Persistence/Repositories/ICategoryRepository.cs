using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid workspaceId, Guid categoryId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Category category, Guid workspaceId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Category category, Guid workspaceId, CancellationToken cancellationToken = default);
}
