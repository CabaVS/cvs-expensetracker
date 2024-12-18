using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceReadRepository
{
    Task<WorkspaceModel[]> GetWorkspaces(Guid userId, CancellationToken cancellationToken = default);
    Task<WorkspaceModel?> GetWorkspaceById(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    
    Task<bool> IsAdmin(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsMemberOfWorkspace(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
}