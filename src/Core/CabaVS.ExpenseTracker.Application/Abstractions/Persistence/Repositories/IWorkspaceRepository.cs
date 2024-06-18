using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceRepository
{
    Task<Workspace?> GetById(Guid workspaceId, Guid userId, CancellationToken ct = default);
    Task<Guid> Create(Workspace workspace, Guid userId, CancellationToken ct = default);
    Task Update(Workspace workspace, Guid userId, CancellationToken ct = default);
    Task Delete(Workspace workspace, Guid userId, CancellationToken ct = default);
}