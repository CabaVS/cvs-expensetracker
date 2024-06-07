using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceReadRepository
{
    Task<WorkspaceModel?> GetById(Guid id, Guid userId, CancellationToken ct = default);
}