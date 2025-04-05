using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface ICategoryRepository
{
    Task<Guid> CreateAsync(Category category, Guid workspaceId, CancellationToken cancellationToken = default);
}
