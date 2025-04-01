using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IReadOnlyWorkspaceRepository
{
    Task<WorkspaceModel[]> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<WorkspaceDetailsModel?> GetDetailsAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken);
    
    Task<bool> UserIsMemberOfWorkspaceAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken);
}
