namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IUserReadRepository
{
    Task<bool> HasAccessToWorkspace(Guid userId, Guid workspaceId, CancellationToken ct = default);
    Task<bool> HasAdminAccessToWorkspace(Guid userId, Guid workspaceId, CancellationToken ct = default);
}