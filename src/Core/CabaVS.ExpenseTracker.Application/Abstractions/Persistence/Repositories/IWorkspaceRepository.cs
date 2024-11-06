using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceRepository
{
    Task<Workspace?> GetById(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    
    Task<Guid> Create(Workspace workspace, CancellationToken cancellationToken = default);
    Task Update(Workspace workspace, CancellationToken cancellationToken = default);
    Task Delete(Workspace workspace, CancellationToken cancellationToken = default);
    
    Task RegisterUser(Guid workspaceId, Guid userId, bool isAdmin, CancellationToken cancellationToken = default);
    Task UnregisterUser(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    Task MakeAnAdmin(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    
    Task<bool> IsAdmin(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
}