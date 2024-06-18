namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICategoryReadRepository
{
    Task<CategoryModel[]> GetAll(Guid workspaceId, CancellationToken ct = default);
    Task<CategoryModel?> GetById(Guid categoryId, Guid workspaceId, CancellationToken ct = default);
}