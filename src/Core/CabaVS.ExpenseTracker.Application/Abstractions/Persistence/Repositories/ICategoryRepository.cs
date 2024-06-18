using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetById(Guid categoryId, Guid workspaceId, CancellationToken ct = default);
    Task<Guid> Create(Category category, Guid workspaceId, CancellationToken ct = default);
    Task Delete(Category category, Guid workspaceId, CancellationToken ct = default);
}