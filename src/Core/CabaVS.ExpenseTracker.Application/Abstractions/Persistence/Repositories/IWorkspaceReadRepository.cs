using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceReadRepository
{
    Task<WorkspaceModel?> GetWorkspaceById(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
}