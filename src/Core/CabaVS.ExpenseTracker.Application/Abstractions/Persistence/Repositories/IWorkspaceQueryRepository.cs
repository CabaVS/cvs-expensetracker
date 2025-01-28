using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceQueryRepository
{
    Task<WorkspaceModel[]> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<WorkspaceModel?> GetByIdAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken = default);
    
    Task<bool> IsMemberOfWorkspaceAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken = default);
    Task<bool> IsAdminOfWorkspaceAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken = default);
}
