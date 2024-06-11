using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceReadRepository
{
    Task<WorkspaceModel[]> GetAll(Guid userId, CancellationToken ct = default);
    Task<WorkspaceModel?> GetById(Guid id, Guid userId, CancellationToken ct = default);
}